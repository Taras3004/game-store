namespace GameStore.Core.Dto.Responses
{
    public record GameResponse
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Key { get; init; }
        public string? Description { get; init; }
        public required double Price { get; init; }
        public required int Discontinued { get; init; }
        public required int UnitInStock { get; init; }
        public required string PublisherId { get; init; }
    }
}
