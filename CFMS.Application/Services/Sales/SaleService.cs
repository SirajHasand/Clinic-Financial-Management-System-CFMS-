using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Sales;
using CFMS.Domain.Entities;
// using Microsoft.EntityFrameworkCore;

namespace CFMS.Application.Services.Sales;

public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SaleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SaleDto> GetByIdAsync(Guid id)
    {
        var sale = await _unitOfWork.Repository<Sale>().GetByIdAsync(id);
        if (sale == null || sale.IsDeleted)
            throw new KeyNotFoundException($"Sale with id {id} not found");
        
        var saleDto = _mapper.Map<SaleDto>(sale);
        
        // Load customer name if exists
        if (sale.CustomerId.HasValue)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(sale.CustomerId.Value);
            saleDto.CustomerName = customer?.FullName;
        }
        
        // Load sale items with drug names
        var saleItems = await _unitOfWork.Repository<SaleItem>()
            .FindAsync(si => si.SaleId == id);
        
        foreach (var item in saleItems)
        {
            var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(item.DrugId);
            saleDto.SaleItems.Add(new SaleItemDto
            {
                Id = item.Id,
                DrugId = item.DrugId,
                DrugName = drug?.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            });
        }
        
        return saleDto;
    }

    public async Task<IEnumerable<SaleDto>> GetAllAsync()
    {
        var sales = await _unitOfWork.Repository<Sale>().GetAllAsync();
        var activeSales = sales.Where(s => !s.IsDeleted);
        return _mapper.Map<IEnumerable<SaleDto>>(activeSales);
    }

    public async Task<SaleDto> CreateAsync(CreateSaleRequest request)
    {
        // Generate invoice number
        var invoiceNumber = GenerateInvoiceNumber();
        
        // Calculate totals
        decimal totalAmount = 0;
        foreach (var item in request.SaleItems)
        {
            totalAmount += item.Quantity * item.UnitPrice;
        }
        
        var remainingAmount = totalAmount - request.PaidAmount;
        
        // Create sale
        var sale = new Sale
        {
            InvoiceNumber = invoiceNumber,
            CustomerId = request.CustomerId,
            TotalAmount = totalAmount,
            PaidAmount = request.PaidAmount,
            RemainingAmount = remainingAmount,
            CreatedAt = DateTime.UtcNow
        };
        
        var createdSale = await _unitOfWork.Repository<Sale>().AddAsync(sale);
        await _unitOfWork.CompleteAsync();
        
        // Create sale items and update inventory
        foreach (var item in request.SaleItems)
        {
            var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(item.DrugId);
            if (drug == null)
                throw new KeyNotFoundException($"Drug with id {item.DrugId} not found");
            
            if (drug.QuantityInStock < item.Quantity)
                throw new InvalidOperationException($"Insufficient stock for drug {drug.Name}. Available: {drug.QuantityInStock}, Requested: {item.Quantity}");
            
            // Update inventory
            drug.QuantityInStock -= item.Quantity;
            await _unitOfWork.Repository<Drug>().UpdateAsync(drug);
            
            // Create sale item
            var saleItem = new SaleItem
            {
                SaleId = createdSale.Id,
                DrugId = item.DrugId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            };
            
            await _unitOfWork.Repository<SaleItem>().AddAsync(saleItem);
        }
        
        await _unitOfWork.CompleteAsync();
        
        // If customer has remaining amount, create a loan record
        if (remainingAmount > 0 && request.CustomerId.HasValue)
        {
            var loan = new Loan
            {
                CustomerId = request.CustomerId.Value,
                LoanAmount = remainingAmount,
                PaidAmount = 0,
                RemainingAmount = remainingAmount,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30) // 30 days due
            };
            
            await _unitOfWork.Repository<Loan>().AddAsync(loan);
            await _unitOfWork.CompleteAsync();
        }
        
        return await GetByIdAsync(createdSale.Id);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var sales = await _unitOfWork.Repository<Sale>()
            .FindAsync(s => s.CreatedAt.Date >= startDate.Date && 
                           s.CreatedAt.Date <= endDate.Date && 
                           !s.IsDeleted);
        return _mapper.Map<IEnumerable<SaleDto>>(sales);
    }

    public async Task<decimal> GetTotalSalesTodayAsync()
    {
        var today = DateTime.UtcNow.Date;
        var sales = await _unitOfWork.Repository<Sale>()
            .FindAsync(s => s.CreatedAt.Date == today && !s.IsDeleted);
        return sales.Sum(s => s.TotalAmount);
    }
    
    private string GenerateInvoiceNumber()
    {
        return $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}