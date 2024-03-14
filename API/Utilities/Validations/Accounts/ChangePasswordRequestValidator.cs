using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .ExclusiveBetween(000000, 999999).WithMessage("OTP must 6 digit value.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New Password is required.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.");
        }
    }
}
