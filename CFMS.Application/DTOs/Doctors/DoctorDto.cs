namespace CFMS.Application.DTOs.Doctors;

public record DoctorDto(
    Guid Id,
    string FullName,
    string Specialization,
    string PhoneNumber,
    string? Email,
    decimal ConsultationFee,
    DateTime CreatedAt
);

public record CreateDoctorRequest(
    string FullName,
    string Specialization,
    string PhoneNumber,
    string? Email,
    decimal ConsultationFee
);

public record UpdateDoctorRequest(
    string FullName,
    string Specialization,
    string PhoneNumber,
    string? Email,
    decimal ConsultationFee
);