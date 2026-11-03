using FluentValidation;
using GameStore.Core.Dto.Details.Genre;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Genre
{
    public class GenreDetailsWithIdValidator : AbstractValidator<GenreDetailsWithId>
    {
        public GenreDetailsWithIdValidator()
        {
            Include(new GenreDetailsBaseValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}
