using FluentValidation;
using GameStore.Core.Dto.Details.Genre;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Details.Genre
{
    public class GenreDetailsBaseValidator : AbstractValidator<GenreDetailsBase>
    {
        public GenreDetailsBaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            int genreNameMaxLength = GenreValidation.NameMaxLength;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(genreNameMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);

            RuleFor(x => x.ParentGenreId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("ParentGenreId must be a valid GUID.");
        }
    }
}
