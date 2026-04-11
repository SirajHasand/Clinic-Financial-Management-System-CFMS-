using Microsoft.AspNetCore.Mvc;
using CFMS.Application.Services.Sales;
using CFMS.Application.DTOs.Sales;

namespace CFMS.WebAPI.Controllers;

public class SalesController : BaseApiController
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll()
    {
        var sales = await _saleService.GetAllAsync();
        return Ok(sales);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetById(Guid id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        return Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create([FromBody] CreateSaleRequest request)
    {
        var sale = await _saleService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
    }

    [HttpGet("today/total")]
    public async Task<ActionResult<decimal>> GetTodayTotal()
    {
        var total = await _saleService.GetTotalSalesTodayAsync();
        return Ok(new { date = DateTime.Today, totalSales = total });
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var sales = await _saleService.GetSalesByDateRangeAsync(startDate, endDate);
        return Ok(sales);
    }
}
