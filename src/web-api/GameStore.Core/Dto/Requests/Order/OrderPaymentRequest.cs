using GameStore.Core.Dto.Details.Payment;

namespace GameStore.Core.Dto.Requests.Order
{
    public record OrderPaymentRequest
    {
        public required string Method { get; init; }
        public PaymentModel? Model { get; init; }
    }
}
