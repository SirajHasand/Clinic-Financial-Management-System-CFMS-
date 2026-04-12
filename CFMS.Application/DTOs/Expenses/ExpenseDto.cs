namespace CFMS.Application.DTOs.Expenses;

public record ExpenseDto(
    Guid Id,
    string Title,
    string? Description,
    decimal Amount,
    DateTime ExpenseDate,
    DateTime CreatedAt
);

public record CreateExpenseRequest(
    string Title,
    string? Description,
    decimal Amount,
    DateTime ExpenseDate
);

public record UpdateExpenseRequest(
    string Title,
    string? Description,
    decimal Amount,
    DateTime ExpenseDate
);
