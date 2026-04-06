using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Drugs;
using CFMS.Domain.Entities;

namespace CFMS.Application.Services.Drugs;

public class DrugService : IDrugService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DrugService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DrugDto> GetByIdAsync(Guid id)
    {
        var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(id);
        if (drug == null || drug.IsDeleted)
            throw new KeyNotFoundException($"Drug with id {id} not found");
        
        return _mapper.Map<DrugDto>(drug);
    }

    public async Task<IEnumerable<DrugDto>> GetAllAsync()
    {
        var drugs = await _unitOfWork.Repository<Drug>().GetAllAsync();
        var activeDrugs = drugs.Where(d => !d.IsDeleted);
        return _mapper.Map<IEnumerable<DrugDto>>(activeDrugs);
    }

    public async Task<DrugDto> CreateAsync(CreateDrugRequest request)
{
    var drug = _mapper.Map<Drug>(request);
    
    // Ensure ExpiryDate is UTC if provided
    if (drug.ExpiryDate.HasValue)
    {
        drug.ExpiryDate = DateTime.SpecifyKind(drug.ExpiryDate.Value, DateTimeKind.Utc);
    }
    
    var created = await _unitOfWork.Repository<Drug>().AddAsync(drug);
    await _unitOfWork.CompleteAsync();
    return _mapper.Map<DrugDto>(created);
}

public async Task<DrugDto> UpdateAsync(Guid id, UpdateDrugRequest request)
{
    var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(id);
    if (drug == null || drug.IsDeleted)
        throw new KeyNotFoundException($"Drug with id {id} not found");

    _mapper.Map(request, drug);
    drug.UpdatedAt = DateTime.UtcNow;
    
    // Ensure ExpiryDate is UTC if provided
    if (drug.ExpiryDate.HasValue)
    {
        drug.ExpiryDate = DateTime.SpecifyKind(drug.ExpiryDate.Value, DateTimeKind.Utc);
    }
    
    await _unitOfWork.Repository<Drug>().UpdateAsync(drug);
    await _unitOfWork.CompleteAsync();
    
    return _mapper.Map<DrugDto>(drug);
}

    
    public async Task DeleteAsync(Guid id)
    {
        var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(id);
        if (drug == null || drug.IsDeleted)
            throw new KeyNotFoundException($"Drug with id {id} not found");

        drug.IsDeleted = true;
        drug.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Drug>().UpdateAsync(drug);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Repository<Drug>().ExistsAsync(d => d.Id == id && !d.IsDeleted);
    }
}