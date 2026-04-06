using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Patients;
using CFMS.Domain.Entities;

namespace CFMS.Application.Services.Patients;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PatientDto> GetByIdAsync(Guid id)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null || patient.IsDeleted)
            throw new KeyNotFoundException($"Patient with id {id} not found");
        
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
        var activePatients = patients.Where(p => !p.IsDeleted);
        return _mapper.Map<IEnumerable<PatientDto>>(activePatients);
    }

    public async Task<PatientDto> CreateAsync(CreatePatientRequest request)
    {
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
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null || patient.IsDeleted)
            throw new KeyNotFoundException($"Patient with id {id} not found");

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
        if (patient == null || patient.IsDeleted)
            throw new KeyNotFoundException($"Patient with id {id} not found");

        patient.IsDeleted = true;
        patient.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Patient>().UpdateAsync(patient);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Repository<Patient>().ExistsAsync(p => p.Id == id && !p.IsDeleted);
    }
}