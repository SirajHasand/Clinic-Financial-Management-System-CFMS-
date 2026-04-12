using FluentValidation;
using CFMS.Application.DTOs.Doctors;

namespace CFMS.Application.Validators.Doctors;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorRequest>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters")
            .MinimumLength(3).WithMessage("Full name must be at least 3 characters");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required")
            .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9+\-() ]+$").WithMessage("Invalid phone number format")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email format")
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.ConsultationFee)
            .GreaterThan(0).WithMessage("Consultation fee must be greater than 0")
            .LessThan(1000000).WithMessage("Consultation fee seems too high, please verify");
    }
}

public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorRequest>
{
    // Update Doctor Validator
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters")
            .MinimumLength(3).WithMessage("Full name must be at least 3 characters");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required")
            .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9+\-() ]+$").WithMessage("Invalid phone number format")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email format")
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.ConsultationFee)
            .GreaterThan(0).WithMessage("Consultation fee must be greater than 0")
            .LessThan(1000000).WithMessage("Consultation fee seems too high, please verify");
    }
}
