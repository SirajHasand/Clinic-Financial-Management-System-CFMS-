using FluentValidation;
using CFMS.Application.DTOs.Drugs;

namespace CFMS.Application.Validators.Drugs;

public class CreateDrugValidator : AbstractValidator<CreateDrugRequest>
{
    public CreateDrugValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Drug name is required")
            .MaximumLength(200).WithMessage("Drug name must not exceed 200 characters")
            .MinimumLength(2).WithMessage("Drug name must be at least 2 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.BatchNumber)
            .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.BatchNumber))
            .WithMessage("Batch number must not exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9\-]+$").When(x => !string.IsNullOrEmpty(x.BatchNumber))
            .WithMessage("Batch number can only contain letters, numbers, and hyphens");

        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future");

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity in stock cannot be negative")
            .LessThan(1000000).WithMessage("Quantity seems too high, please verify");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0).WithMessage("Purchase price must be greater than 0")
            .LessThan(1000000).WithMessage("Purchase price seems too high, please verify");

        RuleFor(x => x.SellingPrice)
            .GreaterThan(0).WithMessage("Selling price must be greater than 0")
            .LessThan(1000000).WithMessage("Selling price seems too high, please verify")
            .GreaterThanOrEqualTo(x => x.PurchasePrice)
            .WithMessage("Selling price must be greater than or equal to purchase price");
    }
}

public class UpdateDrugValidator : AbstractValidator<UpdateDrugRequest>
{
    public UpdateDrugValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Drug name is required")
            .MaximumLength(200).WithMessage("Drug name must not exceed 200 characters")
            .MinimumLength(2).WithMessage("Drug name must be at least 2 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.BatchNumber)
            .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.BatchNumber))
            .WithMessage("Batch number must not exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9\-]+$").When(x => !string.IsNullOrEmpty(x.BatchNumber))
            .WithMessage("Batch number can only contain letters, numbers, and hyphens");

        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future");

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity in stock cannot be negative")
            .LessThan(1000000).WithMessage("Quantity seems too high, please verify");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0).WithMessage("Purchase price must be greater than 0")
            .LessThan(1000000).WithMessage("Purchase price seems too high, please verify");

        RuleFor(x => x.SellingPrice)
            .GreaterThan(0).WithMessage("Selling price must be greater than 0")
            .LessThan(1000000).WithMessage("Selling price seems too high, please verify")
            .GreaterThanOrEqualTo(x => x.PurchasePrice)
            .WithMessage("Selling price must be greater than or equal to purchase price");
    }
}
