using GameStore.Core.Dto.Details.Genre;

namespace GameStore.Core.Dto.Requests.Genre
{
    public record UpdateGenreRequest
    {
        public required GenreDetailsWithId Genre { get; init; }
    }
}
