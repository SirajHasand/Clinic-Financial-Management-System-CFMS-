using CFMS.Application.DTOs.Doctors;

namespace CFMS.Application.Services.Doctors;

public interface IDoctorService
{
    Task<DoctorDto> GetByIdAsync(Guid id);
    Task<IEnumerable<DoctorDto>> GetAllAsync();
    Task<DoctorDto> CreateAsync(CreateDoctorRequest request);
    Task<DoctorDto> UpdateAsync(Guid id, UpdateDoctorRequest request);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}