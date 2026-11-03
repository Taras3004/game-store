namespace GameStore.Core.Dto.Responses
{
    public record GenreResponse : SubGenreResponse
    {
        public string? ParentGenreId { get; init; }
    }
}
