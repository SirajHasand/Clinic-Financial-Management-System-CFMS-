using System;
using CFMS.Domain.Common;

namespace CFMS.Domain.Entities;

public class Sale : SoftDeleteBaseEntity
{
     public string InvoiceNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }

    public List<SaleItem> SaleItems { get; set; } = new();
}
