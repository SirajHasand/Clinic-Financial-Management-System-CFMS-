using System;
using CFMS.Domain.Common;
using CFMS.Domain.Enums;

namespace CFMS.Domain.Entities;

public class Payment : BaseEntity
{
     public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? Note { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

}
