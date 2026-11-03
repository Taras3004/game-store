namespace GameStore.Core.Dto.Responses
{
    public record PaymentMethodResponse
    {
        public required string ImageUrl { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
    }
}
