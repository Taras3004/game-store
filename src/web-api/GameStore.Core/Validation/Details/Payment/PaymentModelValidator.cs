using FluentValidation;
using GameStore.Core.Dto.Details.Payment;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Payment
{
    public class PaymentModelValidator : AbstractValidator<PaymentModel>
    {
        public PaymentModelValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.Holder)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);

            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .CreditCard()
                .WithMessage("Field \"CardNumber\" must contain proper credit card number");

            RuleFor(x => x.YearExpire)
                .GreaterThanOrEqualTo(dateTimeProvider.UtcNow.Year)
                .WithMessage("Credit card have already been expired");

            RuleFor(x => x.MonthExpire)
                .InclusiveBetween(1, 12)
                .WithMessage("Field \"MonthExpire\" must contain value between 1 and 12");


            RuleFor(x => x.Cvv2)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .Matches("^[0-9]{3,4}$")
                .WithMessage("Cvv2 is not valid");
        }
    }
}
