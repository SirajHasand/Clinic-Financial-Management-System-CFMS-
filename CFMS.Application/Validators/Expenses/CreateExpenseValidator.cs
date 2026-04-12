using FluentValidation;
using CFMS.Application.DTOs.Expenses;

namespace CFMS.Application.Validators.Expenses;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Expense title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .LessThan(10000000).WithMessage("Amount seems too high, please verify");

        RuleFor(x => x.ExpenseDate)
            .NotEmpty().WithMessage("Expense date is required")
            .Must(date => date <= DateTime.UtcNow)
            .WithMessage("Expense date cannot be in the future");
    }
}

public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Expense title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .LessThan(10000000).WithMessage("Amount seems too high, please verify");

        RuleFor(x => x.ExpenseDate)
            .NotEmpty().WithMessage("Expense date is required")
            .Must(date => date <= DateTime.UtcNow)
            .WithMessage("Expense date cannot be in the future");
    }
}
