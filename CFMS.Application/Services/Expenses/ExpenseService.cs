using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Expenses;
using CFMS.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CFMS.Application.Services.Expenses;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateExpenseRequest> _createValidator;
    private readonly IValidator<UpdateExpenseRequest> _updateValidator;

    public ExpenseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateExpenseRequest> createValidator,
        IValidator<UpdateExpenseRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ExpenseDto> GetByIdAsync(Guid id)
    {
        var expense = await _unitOfWork.Repository<Expense>().GetByIdAsync(id);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with id {id} not found");

        return _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
    {
        var expenses = await _unitOfWork.Repository<Expense>().GetAllAsync();
        return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
    }

    public async Task<ExpenseDto> CreateAsync(CreateExpenseRequest request)
    {
        // Validate request
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        var expense = _mapper.Map<Expense>(request);

        // Ensure ExpenseDate is UTC
        expense.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);

        var created = await _unitOfWork.Repository<Expense>().AddAsync(expense);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<ExpenseDto>(created);
    }

    public async Task<ExpenseDto> UpdateAsync(Guid id, UpdateExpenseRequest request)
    {
        // Validate request
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        var expense = await _unitOfWork.Repository<Expense>().GetByIdAsync(id);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with id {id} not found");

        _mapper.Map(request, expense);
        expense.UpdatedAt = DateTime.UtcNow;

        // Ensure ExpenseDate is UTC
        expense.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);

        await _unitOfWork.Repository<Expense>().UpdateAsync(expense);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<ExpenseDto>(expense);
    }

    public async Task DeleteAsync(Guid id)
    {
        var expense = await _unitOfWork.Repository<Expense>().GetByIdAsync(id);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with id {id} not found");

        expense.IsDeleted = true;
        expense.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Expense>().UpdateAsync(expense);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var expenses = await _unitOfWork.Repository<Expense>()
            .FindAsync(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate);

        return expenses.Sum(e => e.Amount);
    }

    public async Task<decimal> GetTotalExpensesTodayAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var expenses = await _unitOfWork.Repository<Expense>()
            .FindAsync(e => e.ExpenseDate >= today && e.ExpenseDate < tomorrow);

        return expenses.Sum(e => e.Amount);
    }
}
