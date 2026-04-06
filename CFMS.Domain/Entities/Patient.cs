using System;
using CFMS.Domain.Common;
using CFMS.Domain.Enums;
using CFMS.Domain.ValueObjects;

namespace CFMS.Domain.Entities;

public class Patient : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public string? NationalId { get; set; }


}
