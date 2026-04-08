using CFMS.Domain.Enums;

namespace CFMS.Application.DTOs.Patients;

public class PatientDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
    public string? NationalId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddressDto
{
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
}

public class CreatePatientRequest
{
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
    public string? NationalId { get; set; }
}

public class UpdatePatientRequest
{
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
    public string? NationalId { get; set; }
}