using CFMS.Application.DTOs.Drugs;

namespace CFMS.Application.Services.Drugs;

public interface IDrugService
{
    Task<DrugDto> GetByIdAsync(Guid id);
    Task<IEnumerable<DrugDto>> GetAllAsync();
    Task<DrugDto> CreateAsync(CreateDrugRequest request);
    Task<DrugDto> UpdateAsync(Guid id, UpdateDrugRequest request);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}