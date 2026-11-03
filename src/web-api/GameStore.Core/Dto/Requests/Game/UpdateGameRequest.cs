using GameStore.Core.Dto.Details.Game;

namespace GameStore.Core.Dto.Requests.Game
{
    public record UpdateGameRequest
    {
        public required GameDetailsWithId Game { get; init; }
        public IEnumerable<Guid>? Genres { get; init; }
        public IEnumerable<Guid>? Platforms { get; init; }
        public required Guid Publisher { get; init; }
    }
}
