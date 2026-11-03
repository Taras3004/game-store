using GameStore.Core.Specifications.BaseClass;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task Add(TEntity item, CancellationToken token = default);
        Task Delete(TEntity item, CancellationToken token = default);
        Task Update(TEntity item, CancellationToken token = default);
        Task<TEntity?> GetFirstOrDefaultAsync(Specification<TEntity> specification, CancellationToken token = default);
        Task<IEnumerable<TEntity>> GetListAsync(Specification<TEntity> specification, CancellationToken token = default);
        Task<int> CountAsync(Specification<TEntity> specification, CancellationToken token = default);
        Task<bool> AnyAsync(Specification<TEntity> specification, CancellationToken token = default);
    }
}
