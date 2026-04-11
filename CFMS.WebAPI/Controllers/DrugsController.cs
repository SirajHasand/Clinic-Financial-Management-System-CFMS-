using Microsoft.AspNetCore.Mvc;
using CFMS.Application.Services.Drugs;
using CFMS.Application.DTOs.Drugs;

namespace CFMS.WebAPI.Controllers;

public class DrugsController : BaseApiController
{
    private readonly IDrugService _drugService;

    public DrugsController(IDrugService drugService)
    {
        _drugService = drugService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DrugDto>>> GetAll()
    {
        var drugs = await _drugService.GetAllAsync();
        return Ok(drugs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrugDto>> GetById(Guid id)
    {
        var drug = await _drugService.GetByIdAsync(id);
        return Ok(drug);
    }

    [HttpPost]
    public async Task<ActionResult<DrugDto>> Create([FromBody] CreateDrugRequest request)
    {
        var drug = await _drugService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = drug.Id }, drug);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DrugDto>> Update(Guid id, [FromBody] UpdateDrugRequest request)
    {
        var drug = await _drugService.UpdateAsync(id, request);
        return Ok(drug);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _drugService.DeleteAsync(id);
        return NoContent();
    }
}
