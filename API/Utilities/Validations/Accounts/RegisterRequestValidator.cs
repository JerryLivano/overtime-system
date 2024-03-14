using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Salary)
                .NotEmpty().WithMessage("Salary is required")
                .GreaterThan(0).WithMessage("Salary must be greater than 0");

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Position is required");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required");
        }
    }
}
