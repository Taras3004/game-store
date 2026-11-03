namespace GameStore.Core.Dto.Details.Genre
{
    public record GenreDetailsBase
    {
        public required string Name { get; init; }
        public Guid? ParentGenreId { get; init; }
    }
}