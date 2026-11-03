using FluentValidation;
using GameStore.Core.Dto.Details.Platform;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Platform
{
    public class PlatformDetailsWithIdValidator : AbstractValidator<PlatformDetailsWithId>
    {
        public PlatformDetailsWithIdValidator()
        {
            Include(new PlatformDetailsBaseValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}
