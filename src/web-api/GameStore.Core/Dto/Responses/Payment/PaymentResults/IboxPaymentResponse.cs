using GameStore.Core.Interfaces.Payment;

namespace GameStore.Core.Dto.Responses.Payment.PaymentResults
{
    public record IboxPaymentResponse : IPaymentResponse
    {
        public required Guid UserId { get; init; }
        public required Guid OrderId { get; init; }
        public required DateTime PaymentDate { get; init; }
        public required double Sum { get; init; }
    }
}
