using AutoMapper;
using CFMS.Application.Common.Interfaces;
using CFMS.Application.DTOs.Doctors;
using CFMS.Domain.Entities;

namespace CFMS.Application.Services.Doctors;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IMapper _mapper;

    // public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IDoctorRepository )
    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DoctorDto> GetByIdAsync(Guid id)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null || doctor.IsDeleted)
            throw new KeyNotFoundException($"Doctor with id {id} not found");
        
        return _mapper.Map<DoctorDto>(doctor);
    }
    // doctor.IsDeleted to db level
    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync();
        var activeDoctors = doctors.Where(d => !d.IsDeleted);
        return _mapper.Map<IEnumerable<DoctorDto>>(activeDoctors);
    }


// validation
    public async Task<DoctorDto> CreateAsync(CreateDoctorRequest request)
{
    var doctor = _mapper.Map<Doctor>(request);
    var created = await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
    await _unitOfWork.CompleteAsync();
    return _mapper.Map<DoctorDto>(created);
}

    public async Task<DoctorDto> UpdateAsync(Guid id, UpdateDoctorRequest request)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null || doctor.IsDeleted)
            throw new KeyNotFoundException($"Doctor with id {id} not found");

        _mapper.Map(request, doctor);
        doctor.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Doctor>().UpdateAsync(doctor);
        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task DeleteAsync(Guid id)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null || doctor.IsDeleted)
            throw new KeyNotFoundException($"Doctor with id {id} not found");

        doctor.IsDeleted = true;
        doctor.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Repository<Doctor>().UpdateAsync(doctor);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Repository<Doctor>().ExistsAsync(d => d.Id == id && !d.IsDeleted);
    }
}