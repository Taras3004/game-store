using FluentValidation;
using GameStore.Core.Dto.Details.Publisher;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Validation.Details.Publisher
{
    public class PublisherDetailsBaseValidator : AbstractValidator<PublisherDetailsBase>
    {
        public PublisherDetailsBaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            int companyNameMaxLength = PublisherValidation.CompanyNameMaxLength;

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .MaximumLength(companyNameMaxLength)
                .WithMessage(ValidationMessages.LengthHigherThanMaxValue);
        }
    }
}
