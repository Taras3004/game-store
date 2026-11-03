using FluentValidation;
using GameStore.Core.Dto.Requests.Platform;
using GameStore.Core.Validation.Details.Platform;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Requests.Platform
{
    public class CreatePlatformRequestValidator : AbstractValidator<CreatePlatformRequest>
    {
        public CreatePlatformRequestValidator()
        {
            RuleFor(x => x.Platform)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new PlatformDetailsBaseValidator());
        }
    }
}
