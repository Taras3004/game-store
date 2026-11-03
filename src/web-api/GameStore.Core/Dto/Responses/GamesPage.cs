namespace GameStore.Core.Dto.Responses
{
    public record GamesPage
    {
        public required IEnumerable<GameResponse> Games { get; init; }
        public int TotalPages { get; init; }
        public int CurrentPage { get; init; }
    }
}