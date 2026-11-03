namespace GameStore.Core.Dto.Details.Game
{
    public record GameDetailsBase
    {
        public required string Name { get; init; }
        public string? Key { get; init; }
        public string? Description { get; init; }
        public required double Price { get; init; }
        public required int Discount { get; init; }
        public required int UnitInStock { get; init; }
    }
}