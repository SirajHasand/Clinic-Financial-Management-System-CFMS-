using FluentValidation;
using CFMS.Application.DTOs.Patients;

namespace CFMS.Application.Validators.Patients;

public class CreatePatientValidator : AbstractValidator<CreatePatientRequest>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9+\-() ]+$").WithMessage("Invalid phone number format");

        RuleFor(x => x.NationalId)
            .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage("National ID must not exceed 20 characters");

        RuleFor(x => x.DateOfBirth)
            .Must(date => date <= DateTime.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth cannot be in the future");
    }
}