namespace GameStore.Core.Dto.Details.Publisher
{
    public record PublisherDetailsBase
    {
        public required string CompanyName { get; init; }
        public string? HomePage { get; init; }
        public string? Description { get; init; }
    }
}
