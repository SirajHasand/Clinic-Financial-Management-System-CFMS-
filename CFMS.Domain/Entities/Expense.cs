using System;
using CFMS.Domain.Common;

namespace CFMS.Domain.Entities;

public class Expense : SoftDeleteBaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
}
