using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface IGameRepository
    {
        Task<Game?> GetByIdAsync(Guid id, GamePermissionDetails? permissions = null, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);
        Task<Game?> GetByKeyAsync(string key, GamePermissionDetails? permissions = null, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);

        Task<IEnumerable<Game>> GetByFilterAsync(GameFilterDetails filter, GamePermissionDetails permissions, CancellationToken token = default);
        Task<IEnumerable<Game>> GetByPlatformIdAsync(Guid platformId, GamePermissionDetails permissions, CancellationToken token = default);
        Task<IEnumerable<Game>> GetByGenreIdAsync(Guid genreId, GamePermissionDetails permissions, CancellationToken token = default);
        Task<IEnumerable<Game>> GetByPublisherCompanyNameAsync(string companyName, GamePermissionDetails permissions, CancellationToken token = default);

        Task<int> GetCountAsync(GameFilterDetails? filter = null, GamePermissionDetails? permissions = null, CancellationToken token = default);
        Task<bool> IsKeyTakenAsync(string key, Guid? excludeId = null, CancellationToken token = default);

        Task<Game?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default);
        Task<Game?> GetOrMigrateByKeyAsync(string key, CancellationToken token = default);

        Task AddAsync(Game game, CancellationToken token = default);
        Task UpdateAsync(Game game, CancellationToken token = default);
        Task DeleteAsync(Game game, CancellationToken token = default);

        Task IncrementViewsByIdAsync(Guid id, CancellationToken token = default);
        Task IncrementViewsByKeyAsync(string key, CancellationToken token = default);
        Task IncrementCommentCountAsync(string key, CancellationToken token = default);
        Task SyncStockAsync(string key, int newStock, CancellationToken token = default);
    }
}