namespace GameStore.Core.Payment.Options.PaymentGateway
{
    public record PaymentGatewayOptions
    {
        public const string SectionName = "PaymentGateway";

        public required IEnumerable<PaymentMethodInfo> PaymentMethods { get; init; }
    }
}
