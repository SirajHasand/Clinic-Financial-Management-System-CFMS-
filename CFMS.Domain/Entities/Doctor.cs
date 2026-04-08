using System;
using CFMS.Domain.Common;

namespace CFMS.Domain.Entities;

public class Doctor : SoftDeleteBaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public decimal ConsultationFee { get; set; }
}