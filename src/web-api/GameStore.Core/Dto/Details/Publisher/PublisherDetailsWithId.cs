namespace GameStore.Core.Dto.Details.Publisher
{
    public record PublisherDetailsWithId : PublisherDetailsBase
    {
        public required Guid Id { get; init; }
    }
}
