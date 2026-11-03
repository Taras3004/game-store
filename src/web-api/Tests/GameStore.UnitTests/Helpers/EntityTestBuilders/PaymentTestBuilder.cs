using GameStore.Core.Dto.Requests.Order;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class PaymentTestBuilder
    {
        public static OrderPaymentRequest CreateVisaRequest() => new()
        {
            Method = "Visa"
        };
    }
}
