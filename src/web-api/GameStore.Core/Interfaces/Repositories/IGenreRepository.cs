using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface IGenreRepository
    {
        Task<Genre?> GetByIdAsync(Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);

        Task<Genre?> GetByNameAsync(string name, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);

        Task<IEnumerable<Genre>> GetAllAsync(CancellationToken token = default);

        Task<IEnumerable<Genre>> GetByParentIdAsync(Guid parentId, CancellationToken token = default);

        Task<IEnumerable<Genre>> GetByGameKeyAsync(string gameKey, CancellationToken token = default);

        Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null, CancellationToken token = default);

        Task<List<Genre>> GetOrMigrateByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default);

        Task AddAsync(Genre genre, CancellationToken token = default);
        Task UpdateAsync(Genre genre, CancellationToken token = default);
        Task DeleteAsync(Genre genre, CancellationToken token = default);
    }
}
