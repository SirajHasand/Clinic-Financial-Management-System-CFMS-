using CFMS.Application.DTOs.Patients;

namespace CFMS.Application.Services.Patients;

public interface IPatientService
{
    Task<PatientDto> GetByIdAsync(Guid id);
    Task<IEnumerable<PatientDto>> GetAllAsync();
    Task<PatientDto> CreateAsync(CreatePatientRequest request);
    Task<PatientDto> UpdateAsync(Guid id, UpdatePatientRequest request);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}