using FluentValidation;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Game
{
    public class GameDetailsWithIdValidator : AbstractValidator<GameDetailsWithId>
    {
        public GameDetailsWithIdValidator()
        {
            Include(new GameDetailsBaseValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}
