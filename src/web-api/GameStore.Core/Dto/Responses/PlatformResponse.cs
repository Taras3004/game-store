namespace GameStore.Core.Dto.Responses
{
    public record PlatformResponse
    {
        public required Guid Id { get; init; }
        public required string Type { get; init; }
    }
}
