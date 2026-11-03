using FluentValidation;
using GameStore.Core.Dto.Details.Platform;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Details.Platform
{
    public class PlatformDetailsBaseValidator : AbstractValidator<PlatformDetailsBase>
    {
        public PlatformDetailsBaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            int platformTypeMaxLength = PlatformValidation.TypeMaxLength;

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(platformTypeMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);
        }
    }
}
