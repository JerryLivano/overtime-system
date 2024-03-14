using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validations.Employees
{
    public class EmployeeRequestValidator : AbstractValidator<EmployeeRequestDto>
    {
        public EmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(1, 50).WithMessage("Character must be within 1 - 50.");

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
        }
    }
}
