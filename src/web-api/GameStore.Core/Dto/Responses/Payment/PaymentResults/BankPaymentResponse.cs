using GameStore.Core.Interfaces.Payment;

namespace GameStore.Core.Dto.Responses.Payment.PaymentResults
{
    public record BankPaymentResponse : IPaymentResponse
    {
        public required byte[] FileContent { get; init; }
        public required string ContentType { get; init; }
        public required string FileName { get; init; }
    }
}
