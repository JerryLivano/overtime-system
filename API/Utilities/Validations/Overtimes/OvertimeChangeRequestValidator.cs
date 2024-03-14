using API.DTOs.Overtimes;
using FluentValidation;

namespace API.Utilities.Validations.Overtimes
{
    public class OvertimeChangeRequestValidator : AbstractValidator<OvertimeChangeRequestDto>
    {
        public OvertimeChangeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.");
        }
    }
}
