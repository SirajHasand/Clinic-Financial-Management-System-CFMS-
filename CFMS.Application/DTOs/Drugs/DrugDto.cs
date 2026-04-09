namespace CFMS.Application.DTOs.Drugs;

public record DrugDto(
    Guid Id,
    string Name,
    string? Description,
    string? BatchNumber,
    DateTime? ExpiryDate,
    int QuantityInStock,
    decimal PurchasePrice,
    decimal SellingPrice,
    DateTime CreatedAt
);

public record CreateDrugRequest(
    string Name,
    string? Description,
    string? BatchNumber,
    DateTime? ExpiryDate,
    int QuantityInStock,
    decimal PurchasePrice,
    decimal SellingPrice
);

public record UpdateDrugRequest(
    string Name,
    string? Description,
    string? BatchNumber,
    DateTime? ExpiryDate,
    int QuantityInStock,
    decimal PurchasePrice,
    decimal SellingPrice
);