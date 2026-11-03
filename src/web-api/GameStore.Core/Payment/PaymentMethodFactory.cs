using GameStore.Core.Interfaces.Payment;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Core.Payment
{
    internal class PaymentMethodFactory(IServiceProvider serviceProvider) : IPaymentMethodFactory
    {
        public IPaymentMethod GetStrategy(string methodName)
        {
            var strategy = serviceProvider.GetKeyedService<IPaymentMethod>(methodName);

            return strategy ?? throw new NotSupportedException($"Payment method '{methodName}' is not supported.");
        }
    }
}
