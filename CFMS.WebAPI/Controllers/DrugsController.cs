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
        try
        {
            var drug = await _drugService.GetByIdAsync(id);
            return Ok(drug);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<DrugDto>> Create([FromBody] CreateDrugRequest request)
    {
        try
        {
            var drug = await _drugService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = drug.Id }, drug);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DrugDto>> Update(Guid id, [FromBody] UpdateDrugRequest request)
    {
        try
        {
            var drug = await _drugService.UpdateAsync(id, request);
            return Ok(drug);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _drugService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}