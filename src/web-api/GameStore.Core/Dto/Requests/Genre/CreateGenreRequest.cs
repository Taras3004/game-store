using GameStore.Core.Dto.Details.Genre;

namespace GameStore.Core.Dto.Requests.Genre
{
    public record CreateGenreRequest
    {
        public required GenreDetailsBase Genre { get; init; }
    }
}
