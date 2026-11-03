using FluentValidation;
using GameStore.Core.Payment.Options.PaymentGateway;
using GameStore.Core.Validation.Helpers;

namespace GameStore.Core.Validation.Details.Payment
{
    public class PaymentMethodInfoValidator : AbstractValidator<PaymentMethodInfo>
    {
        public PaymentMethodInfoValidator()
        {
            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty);
        }
    }
}
