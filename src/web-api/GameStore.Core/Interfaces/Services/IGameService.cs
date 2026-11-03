using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Dto.Requests.Game;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IGameService
    {
        Task<Result<GameResponse>> CreateGame(CreateGameRequest request, CancellationToken token = default);
        Task<Result> UpdateGame(UpdateGameRequest request, CancellationToken token = default);
        Task<Result> DeleteGame(string key, CancellationToken token = default);

        Task<Result<GameResponse>> GetGameByKey(string key, CancellationToken token = default);
        Task<Result<GameResponse>> GetGameById(Guid gameId, CancellationToken token = default);
        Task<Result<GamesPage>> GetAllGames(GameFilterDetails gameFilterDetails, CancellationToken token = default);
        Task<Result<GameFileResponse>> DownloadGameFile(string key, CancellationToken token = default);
        Task<int> GetGameCount(GameFilterDetails? filter = null, CancellationToken token = default);
        Task<IEnumerable<GameResponse>> GetGamesByGenreId(Guid genreId, CancellationToken token = default);
        Task<IEnumerable<GameResponse>> GetGamesByPlatformId(Guid platformId, CancellationToken token = default);
        Task<IEnumerable<GameResponse>> GetGamesByPublisherCompanyName(string companyName, CancellationToken token = default);
        IEnumerable<string> GetSortingOptions();
        IEnumerable<string> GetPaginationOptions();
        IEnumerable<string> GetDateFilteringOptions();
    }
}
