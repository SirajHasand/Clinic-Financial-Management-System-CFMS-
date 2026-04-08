using System;
using CFMS.Domain.Common;
using CFMS.Domain.ValueObjects;

namespace CFMS.Domain.Entities;

public class Customer : SoftDeleteBaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Address? Address { get; set; } 
}

