using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Doctors;
using CFMS.Domain.Entities;
using FluentValidation;

namespace CFMS.Application.Services.Doctors;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IMapper _mapper;
    private readonly IValidator<CreateDoctorRequest> _createValidator;
    private readonly IValidator<UpdateDoctorRequest> _updateValidator;

    public DoctorService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IValidator<CreateDoctorRequest> createValidator,
        IValidator<UpdateDoctorRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<DoctorDto> GetByIdAsync(Guid id)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null)
            throw new KeyNotFoundException($"Doctor with id {id} not found");
        
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorRequest request)
    {
        // Validate request
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        // Check if email already exists
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailExists = await _unitOfWork.Repository<Doctor>()
                .ExistsAsync(d => d.Email == request.Email);
            if (emailExists)
                throw new InvalidOperationException($"A doctor with email '{request.Email}' already exists");
        }

        // Check if phone already exists
        var phoneExists = await _unitOfWork.Repository<Doctor>()
            .ExistsAsync(d => d.PhoneNumber == request.PhoneNumber);
        if (phoneExists)
            throw new InvalidOperationException($"A doctor with phone number '{request.PhoneNumber}' already exists");

        var doctor = _mapper.Map<Doctor>(request);
        var created = await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<DoctorDto>(created);
    }

    public async Task<DoctorDto> UpdateAsync(Guid id, UpdateDoctorRequest request)
    {
        // Validate request
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null)
            throw new KeyNotFoundException($"Doctor with id {id} not found");

        // Check if email already exists (excluding current doctor)
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailExists = await _unitOfWork.Repository<Doctor>()
                .ExistsAsync(d => d.Email == request.Email && d.Id != id);
            if (emailExists)
                throw new InvalidOperationException($"A doctor with email '{request.Email}' already exists");
        }

        // Check if phone already exists (excluding current doctor)
        var phoneExists = await _unitOfWork.Repository<Doctor>()
            .ExistsAsync(d => d.PhoneNumber == request.PhoneNumber && d.Id != id);
        if (phoneExists)
            throw new InvalidOperationException($"A doctor with phone number '{request.PhoneNumber}' already exists");

        _mapper.Map(request, doctor);
        doctor.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Doctor>().UpdateAsync(doctor);
        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task DeleteAsync(Guid id)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null)
            throw new KeyNotFoundException($"Doctor with id {id} not found");

        doctor.IsDeleted = true;
        doctor.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Doctor>().UpdateAsync(doctor);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Repository<Doctor>().ExistsAsync(d => d.Id == id);
    }
}