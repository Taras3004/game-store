namespace GameStore.Core.Dto.Responses
{
    public record OrderProductResponse
    {
        public required string ProductKey { get; init; }
        public int Quantity { get; init; }
    }
}
