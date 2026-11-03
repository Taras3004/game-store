namespace GameStore.Core.Dto.Details.Genre
{
    public record GenreDetailsWithId : GenreDetailsBase
    {
        public required Guid Id { get; init; }
    }
}