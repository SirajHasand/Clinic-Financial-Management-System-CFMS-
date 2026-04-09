using CFMS.Domain.Enums;

namespace CFMS.Application.DTOs.Patients;

public record PatientDto(
    Guid Id,
    string FullName,
    Gender Gender,
    DateTime? DateOfBirth,
    string PhoneNumber,
    AddressDto? Address,
    string? NationalId,
    DateTime CreatedAt
);

public record AddressDto(
    string Province,
    string District,
    string Street
);

public record CreatePatientRequest(
    string FullName,
    Gender Gender,
    DateTime? DateOfBirth,
    string PhoneNumber,
    AddressDto? Address,
    string? NationalId
);

public record UpdatePatientRequest(
    string FullName,
    Gender Gender,
    DateTime? DateOfBirth,
    string PhoneNumber,
    AddressDto? Address,
    string? NationalId
);