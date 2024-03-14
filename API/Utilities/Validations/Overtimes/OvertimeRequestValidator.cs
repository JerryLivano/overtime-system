using API.DTOs.Overtimes;
using FluentValidation;

namespace API.Utilities.Validations.Overtimes
{
    public class OvertimeRequestValidator : AbstractValidator<OvertimeRequestDto>
    {
        public OvertimeRequestValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("DateTime is required.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.");

            RuleFor(x => x.TotalHours)
                .NotEmpty().WithMessage("TotalHours is required.")
                .LessThanOrEqualTo(4).WithMessage("Total hours cannot greater than 4.");
        }
    }
}
