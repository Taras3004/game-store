using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class PaymentErrors
    {
        public static readonly Error UnknownPaymentFailure = new(
            "Payment.UnknownPaymentFailure", 
            "Payment failed. Try again",
            ErrorType.Failure);
    }
}
