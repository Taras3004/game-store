using FluentValidation;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Details.Game
{
    public class GameDetailsBaseValidator : AbstractValidator<GameDetailsBase>
    {
        public GameDetailsBaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            int nameMaxLength = GameValidation.NameMaxLength;
            int keyMaxLength = GameValidation.KeyMaxLength;
            int descriptionMaxLength = GameValidation.DescriptionMaxLength;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(nameMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue)
                .Must(name => name.Any(char.IsLetterOrDigit))
                .WithMessage("Game name must contain at least one letter or digit.");

            RuleFor(x => x.Key)
                .MaximumLength(keyMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue)
                .Matches(@"^[A-Za-z0-9\-]*$")
                .WithMessage("Key can contain only letters, digits and hyphen (-).");

            RuleFor(x => x.Description)
                .MaximumLength(descriptionMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);
        }
    }
}
