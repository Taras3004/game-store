namespace GameStore.Core.Dto.Responses
{
    public record OrderResponse
    {
        public required string Id { get; init; }
        public required string CustomerId { get; init; }
        public DateTime? Date { get; init; }
    }
}
