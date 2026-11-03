namespace GameStore.Core.Dto.Responses
{
    public record PublisherResponse
    {
        public required string Id { get; init; }
        public required string CompanyName { get; init; }
        public string? HomePage { get; init; }
        public string? Description { get; init; }
    }
}
