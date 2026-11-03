namespace GameStore.Core.Payment.Options.PaymentGateway
{
    public record PaymentMethodInfo
    {
        public required string ImageUrl { get; init; }
        public required string Title { get; init; }
        public required string Key { get; init; }
        public required string Description { get; init; }
        public required bool RequireDetails { get; init; }
    }
}
