using System;
using CFMS.Domain.Common;

namespace CFMS.Domain.Entities;

public class Drug : BaseEntity
{
      public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public int QuantityInStock { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }

}
