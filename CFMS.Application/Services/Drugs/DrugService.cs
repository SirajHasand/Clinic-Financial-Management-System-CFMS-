using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Drugs;
using CFMS.Domain.Entities;
using FluentValidation;

namespace CFMS.Application.Services.Drugs;

public class DrugService : IDrugService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateDrugRequest> _createValidator;
    private readonly IValidator<UpdateDrugRequest> _updateValidator;

    public DrugService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IValidator<CreateDrugRequest> createValidator,
        IValidator<UpdateDrugRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<DrugDto> GetByIdAsync(Guid id)
    {
        var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(id);
        if (drug == null)
            throw new KeyNotFoundException($"Drug with id {id} not found");
        
        return _mapper.Map<DrugDto>(drug);
    }

    public async Task<IEnumerable<DrugDto>> GetAllAsync()
    {
        var drugs = await _unitOfWork.Repository<Drug>().GetAllAsync();
        return _mapper.Map<IEnumerable<DrugDto>>(drugs);
    }

    public async Task<DrugDto> CreateAsync(CreateDrugRequest request)
    {
        // Validate request
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        // Check if drug name already exists
        var nameExists = await _unitOfWork.Repository<Drug>()
            .ExistsAsync(d => d.Name.ToLower() == request.Name.ToLower());
        if (nameExists)
            throw new InvalidOperationException($"A drug with name '{request.Name}' already exists");

        // Check if batch number already exists (if provided)
        if (!string.IsNullOrEmpty(request.BatchNumber))
        {
            var batchExists = await _unitOfWork.Repository<Drug>()
                .ExistsAsync(d => d.BatchNumber == request.BatchNumber);
            if (batchExists)
                throw new InvalidOperationException($"A drug with batch number '{request.BatchNumber}' already exists");
        }

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
        // Validate request
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        var drug = await _unitOfWork.Repository<Drug>().GetByIdAsync(id);
        if (drug == null)
            throw new KeyNotFoundException($"Drug with id {id} not found");

        // Check if drug name already exists (excluding current drug)
        var nameExists = await _unitOfWork.Repository<Drug>()
            .ExistsAsync(d => d.Name.ToLower() == request.Name.ToLower() && d.Id != id);
        if (nameExists)
            throw new InvalidOperationException($"A drug with name '{request.Name}' already exists");

        // Check if batch number already exists (excluding current drug)
        if (!string.IsNullOrEmpty(request.BatchNumber))
        {
            var batchExists = await _unitOfWork.Repository<Drug>()
                .ExistsAsync(d => d.BatchNumber == request.BatchNumber && d.Id != id);
            if (batchExists)
                throw new InvalidOperationException($"A drug with batch number '{request.BatchNumber}' already exists");
        }

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
        if (drug == null)
            throw new KeyNotFoundException($"Drug with id {id} not found");

        drug.IsDeleted = true;
        drug.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Drug>().UpdateAsync(drug);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Repository<Drug>().ExistsAsync(d => d.Id == id);
    }
}