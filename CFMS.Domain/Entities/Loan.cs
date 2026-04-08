using System;
using CFMS.Domain.Common;

namespace CFMS.Domain.Entities;

public class Loan : SoftDeleteBaseEntity
{
     public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public decimal LoanAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }

    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }

}
