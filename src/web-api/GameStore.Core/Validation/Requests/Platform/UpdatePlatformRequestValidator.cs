using FluentValidation;
using GameStore.Core.Dto.Requests.Platform;
using GameStore.Core.Validation.Details.Platform;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Requests.Platform
{
    public class UpdatePlatformRequestValidator : AbstractValidator<UpdatePlatformRequest>
    {
        public UpdatePlatformRequestValidator()
        {
            RuleFor(x => x.Platform)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new PlatformDetailsWithIdValidator());
        }
    }
}
