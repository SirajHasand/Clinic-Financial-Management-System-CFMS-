using CFMS.Application.DTOs.Expenses;

namespace CFMS.Application.Services.Expenses;

public interface IExpenseService
{
    Task<ExpenseDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ExpenseDto>> GetAllAsync();
    Task<ExpenseDto> CreateAsync(CreateExpenseRequest request);
    Task<ExpenseDto> UpdateAsync(Guid id, UpdateExpenseRequest request);
    Task DeleteAsync(Guid id);
    Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalExpensesTodayAsync();
}
