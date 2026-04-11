using Microsoft.AspNetCore.Mvc;
using CFMS.Application.Services.Patients;
using CFMS.Application.DTOs.Patients;

namespace CFMS.WebAPI.Controllers;

public class PatientsController : BaseApiController
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Get all patients
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    /// <summary>
    /// Get patient by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetById(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return Ok(patient);
    }

    /// <summary>
    /// Create a new patient
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientRequest request)
    {
        var patient = await _patientService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    /// <summary>
    /// Update an existing patient
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientDto>> Update(Guid id, [FromBody] UpdatePatientRequest request)
    {
        var patient = await _patientService.UpdateAsync(id, request);
        return Ok(patient);
    }

    /// <summary>
    /// Delete a patient (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }
}
