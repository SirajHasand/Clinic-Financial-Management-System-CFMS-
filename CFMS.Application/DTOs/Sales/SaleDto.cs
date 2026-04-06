using CFMS.Domain.Enums;

namespace CFMS.Application.DTOs.Sales;

public class SaleDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public List<SaleItemDto> SaleItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class SaleItemDto
{
    public Guid Id { get; set; }
    public Guid DrugId { get; set; }
    public string? DrugName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CreateSaleRequest
{
    public Guid? CustomerId { get; set; }
    public decimal PaidAmount { get; set; }
    public List<CreateSaleItemRequest> SaleItems { get; set; } = new();
}

public class CreateSaleItemRequest
{
    public Guid DrugId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}