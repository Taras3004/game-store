namespace GameStore.Core.Payment.Options
{
    public record PaymentOptions
    {
        public const string SectionName = "PaymentOptions";

        public required int PaymentValidityDurationDays { get; init; }
    }
}
