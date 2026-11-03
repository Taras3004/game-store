using FluentValidation;
using GameStore.Core.Dto.Requests.Order;
using GameStore.Core.Payment.Options.PaymentGateway;
using GameStore.Core.Validation.Helpers;
using GameStore.Models.Constants.ValidationValues;
using Microsoft.Extensions.Options;

namespace GameStore.Core.Validation.Requests.Payment
{
    public class OrderPaymentRequestValidator : AbstractValidator<OrderPaymentRequest>
    {
        public OrderPaymentRequestValidator(IOptionsSnapshot<PaymentGatewayOptions> paymentOptions)
        {
            var paymentMethods = paymentOptions.Value.PaymentMethods
                .Select(x => x.Key);

            RuleFor(x => x.Method)
                .NotEmpty()
                .WithMessage(ValidationMessages.CanNotBeEmpty)
                .Must(paymentMethods.Contains)
                .WithMessage(PaymentValidation.PaymentMethodIsNotRegistered);

            RuleFor(x => x.Model)
                .NotNull()
                .When(x =>
                {
                    var methodInfo = paymentOptions.Value.PaymentMethods.FirstOrDefault(m => m.Key == x.Method);
                    return methodInfo?.RequireDetails == true;
                });

            RuleFor(x => x.Model)
                .Null()
                .When(x =>
                {
                    var methodInfo = paymentOptions.Value.PaymentMethods.FirstOrDefault(m => m.Key == x.Method);
                    return methodInfo?.RequireDetails == false;
                });
        }
    }
}
