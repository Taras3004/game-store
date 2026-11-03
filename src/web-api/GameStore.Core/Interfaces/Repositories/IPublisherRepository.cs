using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface IPublisherRepository
    {
        Task<Publisher?> GetByIdAsync(Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);
        Task<Publisher?> GetByCompanyNameAsync(string companyName, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);
        Task<IEnumerable<Publisher>> GetAllAsync(CancellationToken token = default);
        Task<Publisher?> GetByGameKeyAsync(string gameKey, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);
        Task<Publisher?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> IsNameTakenAsync(string companyName, CancellationToken token = default);
        Task<bool> IsNameTakenExcludeIdAsync(string companyName, Guid excludeId, CancellationToken token = default);

        Task AddAsync(Publisher publisher, CancellationToken token = default);
        Task UpdateAsync(Publisher publisher, CancellationToken token = default);
        Task DeleteAsync(Publisher publisher, CancellationToken token = default);
    }
}
