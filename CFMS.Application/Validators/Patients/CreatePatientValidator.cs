using FluentValidation;
using CFMS.Application.DTOs.Patients;

namespace CFMS.Application.Validators.Patients;

public class CreatePatientValidator : AbstractValidator<CreatePatientRequest>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters")
            .MinimumLength(3).WithMessage("Full name must be at least 3 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9+\-() ]+$").WithMessage("Invalid phone number format")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

        RuleFor(x => x.NationalId)
            .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage("National ID must not exceed 20 characters")
            .Matches(@"^[0-9a-zA-Z\-]+$").When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage("National ID can only contain letters, numbers, and hyphens");

        RuleFor(x => x.DateOfBirth)
            .Must(date => date <= DateTime.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth cannot be in the future");

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!.Province)
                .MaximumLength(100).When(x => x.Address != null && !string.IsNullOrEmpty(x.Address.Province))
                .WithMessage("Province must not exceed 100 characters");

            RuleFor(x => x.Address!.District)
                .MaximumLength(100).When(x => x.Address != null && !string.IsNullOrEmpty(x.Address.District))
                .WithMessage("District must not exceed 100 characters");

            RuleFor(x => x.Address!.Street)
                .MaximumLength(200).When(x => x.Address != null && !string.IsNullOrEmpty(x.Address.Street))
                .WithMessage("Street must not exceed 200 characters");
        });
    }
}

public class UpdatePatientValidator : AbstractValidator<UpdatePatientRequest>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters")
            .MinimumLength(3).WithMessage("Full name must be at least 3 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9+\-() ]+$").WithMessage("Invalid phone number format")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

        RuleFor(x => x.NationalId)
            .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage("National ID must not exceed 20 characters")
            .Matches(@"^[0-9a-zA-Z\-]+$").When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage("National ID can only contain letters, numbers, and hyphens");

        RuleFor(x => x.DateOfBirth)
            .Must(date => date <= DateTime.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth cannot be in the future");

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!.Province)
                .MaximumLength(100).When(x => x.Address != null && !string.IsNullOrEmpty(x.Address.Province))
                .WithMessage("Province must not exceed 100 characters");

            RuleFor(x => x.Address!.District)
                .MaximumLength(100).When(x => x.Address != null && !string.IsNullOrEmpty(x.Address.District))
                .WithMessage("District must not exceed 100 characters");

            RuleFor(x => x.Address!.Street)
                .MaximumLength(200).When(x => x.Address != null && !string.IsNullOrEmpty(x.Address.Street))
                .WithMessage("Street must not exceed 200 characters");
        });
    }
}