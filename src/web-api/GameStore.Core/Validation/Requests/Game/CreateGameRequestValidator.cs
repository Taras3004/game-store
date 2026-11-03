using FluentValidation;
using GameStore.Core.Dto.Requests.Game;
using GameStore.Core.Validation.Details.Game;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Requests.Game
{
    public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
    {
        public CreateGameRequestValidator()
        {
            RuleFor(x => x.Game)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new GameDetailsBaseValidator());

            When(x => x.Genres != null, () =>
            {
                RuleForEach(x => x.Genres)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.CanNotBeEmpty);

                RuleFor(x => x.Genres)
                    .Must(list =>
                    {
                        IEnumerable<Guid> guids = list!.ToList();
                        return guids.Distinct().Count() == guids.Count();
                    })
                    .WithMessage(GameValidation.GenresMustNotContainDuplicates);
            });

            When(x => x.Platforms != null, () =>
            {
                RuleForEach(x => x.Platforms)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.CanNotBeEmpty);

                RuleFor(x => x.Platforms)
                    .Must(list =>
                    {
                        IEnumerable<Guid> guids = list!.ToList();
                        return guids.Distinct().Count() == guids.Count();
                    })
                    .WithMessage(GameValidation.PlatformsMustNotContainDuplicates);
            });
        }
    }
}
