using FluentValidation;
using GameStore.Core.Dto.Details.Publisher;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Publisher
{
    public class PublisherDetailsWithIdValidator : AbstractValidator<PublisherDetailsWithId>
    {
        public PublisherDetailsWithIdValidator()
        {
            Include(new PublisherDetailsBaseValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }

    }
}
