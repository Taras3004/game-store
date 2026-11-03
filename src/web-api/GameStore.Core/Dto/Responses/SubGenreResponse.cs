namespace GameStore.Core.Dto.Responses
{
    public record SubGenreResponse
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
    }
}
