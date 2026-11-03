namespace GameStore.Core.Dto.Details.Game
{
    public record GameFilterDetails
    {
        public string? Name { get; init; }
        public IEnumerable<Guid>? Genres { get; init; }
        public IEnumerable<Guid>? Platforms { get; init; }
        public IEnumerable<Guid>? Publishers { get; init; }
        public double? MinPrice { get; init; }
        public double? MaxPrice { get; init; }
        public string? DatePublishing { get; init; }
        public string? PageCount { get; init; }
        public int? Page { get; init; }
        public string? Sort { get; init; }
    }
}