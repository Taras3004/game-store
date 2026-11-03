using FluentValidation;
using GameStore.Core.Dto.Requests.Publisher;
using GameStore.Core.Validation.Details.Publisher;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Requests.Publisher
{
    public class CreatePublisherRequestValidator : AbstractValidator<CreatePublisherRequest>
    {
        public CreatePublisherRequestValidator()
        {
            RuleFor(x => x.Publisher)
                .NotNull()
                .WithMessage(ValidationMessages.CanNotBeNull)
                .SetValidator(new PublisherDetailsBaseValidator());
        }
    }
}
