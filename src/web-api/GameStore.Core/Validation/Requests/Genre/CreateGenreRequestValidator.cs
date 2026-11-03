using FluentValidation;
using GameStore.Core.Dto.Requests.Genre;
using GameStore.Core.Validation.Details.Genre;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Requests.Genre
{
    public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
    {
        public CreateGenreRequestValidator()
        {
            RuleFor(x => x.Genre)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new GenreDetailsBaseValidator());
        }
    }
}
