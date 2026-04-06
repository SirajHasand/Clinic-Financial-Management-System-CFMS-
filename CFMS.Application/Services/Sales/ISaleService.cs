using CFMS.Application.DTOs.Sales;

namespace CFMS.Application.Services.Sales;

public interface ISaleService
{
    Task<SaleDto> GetByIdAsync(Guid id);
    Task<IEnumerable<SaleDto>> GetAllAsync();
    Task<SaleDto> CreateAsync(CreateSaleRequest request);
    Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalSalesTodayAsync();
}