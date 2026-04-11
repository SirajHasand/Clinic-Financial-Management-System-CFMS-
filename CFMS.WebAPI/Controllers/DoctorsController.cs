using Microsoft.AspNetCore.Mvc;
using CFMS.Application.Services.Doctors;
using CFMS.Application.DTOs.Doctors;

namespace CFMS.WebAPI.Controllers;

public class DoctorsController : BaseApiController
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAll()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> Create([FromBody] CreateDoctorRequest request)
    {
        var doctor = await _doctorService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DoctorDto>> Update(Guid id, [FromBody] UpdateDoctorRequest request)
    {
        var doctor = await _doctorService.UpdateAsync(id, request);
        return Ok(doctor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _doctorService.DeleteAsync(id);
        return NoContent();
    }
}
