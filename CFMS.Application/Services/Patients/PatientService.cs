using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Patients;
using CFMS.Domain.Entities;
using FluentValidation;

namespace CFMS.Application.Services.Patients;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePatientRequest> _createValidator;
    private readonly IValidator<UpdatePatientRequest> _updateValidator;

    public PatientService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IValidator<CreatePatientRequest> createValidator,
        IValidator<UpdatePatientRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<PatientDto> GetByIdAsync(Guid id)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null)
            throw new KeyNotFoundException($"Patient with id {id} not found");
        
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

    public async Task<PatientDto> CreateAsync(CreatePatientRequest request)
    {
        // Validate request
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        // Check if NationalId already exists
        if (!string.IsNullOrEmpty(request.NationalId))
        {
            var nationalIdExists = await _unitOfWork.Repository<Patient>()
                .ExistsAsync(p => p.NationalId == request.NationalId);
            if (nationalIdExists)
                throw new InvalidOperationException($"A patient with National ID '{request.NationalId}' already exists");
        }

        // Check if phone already exists
        //
        var phoneExists = await _unitOfWork.Repository<Patient>()
            .ExistsAsync(p => p.PhoneNumber == request.PhoneNumber);
        if (phoneExists)
            throw new InvalidOperationException($"A patient with phone number '{request.PhoneNumber}' already exists");

        var patient = _mapper.Map<Patient>(request);
        
        // Ensure DateOfBirth is UTC if provided
        if (patient.DateOfBirth.HasValue)
        {
            patient.DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth.Value, DateTimeKind.Utc);
        }
        
        var created = await _unitOfWork.Repository<Patient>().AddAsync(patient);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<PatientDto>(created);
    }

    public async Task<PatientDto> UpdateAsync(Guid id, UpdatePatientRequest request)
    {
        // Validate request
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null)
            throw new KeyNotFoundException($"Patient with id {id} not found");

        // Check if NationalId already exists 
        if (!string.IsNullOrEmpty(request.NationalId))
        {
            var nationalIdExists = await _unitOfWork.Repository<Patient>()
                .ExistsAsync(p => p.NationalId == request.NationalId && p.Id != id);
            if (nationalIdExists)
                throw new InvalidOperationException($"A patient with National ID '{request.NationalId}' already exists");
        }

        // Check if phone already exists 
        var phoneExists = await _unitOfWork.Repository<Patient>()
            .ExistsAsync(p => p.PhoneNumber == request.PhoneNumber && p.Id != id);
        if (phoneExists)
            throw new InvalidOperationException($"A patient with phone number '{request.PhoneNumber}' already exists");

        _mapper.Map(request, patient);
        patient.UpdatedAt = DateTime.UtcNow;
        
        // Ensure DateOfBirth is UTC if provided
        if (patient.DateOfBirth.HasValue)
        {
            patient.DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth.Value, DateTimeKind.Utc);
        }
        
        await _unitOfWork.Repository<Patient>().UpdateAsync(patient);
        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task DeleteAsync(Guid id)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null)
            throw new KeyNotFoundException($"Patient with id {id} not found");

        patient.IsDeleted = true;
        patient.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Patient>().UpdateAsync(patient);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Repository<Patient>().ExistsAsync(p => p.Id == id);
    }
}