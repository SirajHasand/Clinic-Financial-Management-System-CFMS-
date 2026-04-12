using Microsoft.AspNetCore.Mvc;
using CFMS.Application.Services.Expenses;
using CFMS.Application.DTOs.Expenses;

namespace CFMS.WebAPI.Controllers;

// Expenses Controller
public class ExpensesController : BaseApiController
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Get all expenses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
    {
        var expenses = await _expenseService.GetAllAsync();
        return Ok(expenses);
    }

    /// <summary>
    /// Get expense by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetById(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        return Ok(expense);
    }

    /// <summary>
    /// Create a new expense
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseRequest request)
    {
        var expense = await _expenseService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    /// <summary>
    /// Update an existing expense
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseDto>> Update(Guid id, [FromBody] UpdateExpenseRequest request)
    {
        var expense = await _expenseService.UpdateAsync(id, request);
        return Ok(expense);
    }

    /// <summary>
    /// Delete an expense (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _expenseService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Get total expenses for today
    /// </summary>
    [HttpGet("today/total")]
    public async Task<ActionResult<decimal>> GetTodayTotal()
    {
        var total = await _expenseService.GetTotalExpensesTodayAsync();
        return Ok(new { date = DateTime.Today, totalExpenses = total });
    }

    /// <summary>
    /// Get total expenses by date range
    /// </summary>
    [HttpGet("date-range/total")]
    public async Task<ActionResult<decimal>> GetTotalByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var total = await _expenseService.GetTotalExpensesByDateRangeAsync(startDate, endDate);
        return Ok(new { startDate, endDate, totalExpenses = total });
    }
}
