using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Data;
using GameStore.Models.Entities.ConfigurationEntities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories.Base
{
    internal class SqlRepository<T>(GameStoreDbContext context) : IRepository<T>
        where T : BaseEntity
    {
        public async Task<T?> GetFirstOrDefaultAsync(
            Specification<T> specification,
            CancellationToken token = default)
        {
            var dbSet = context.Set<T>();

            var query = EfSpecificationEvaluator.GetQuery(dbSet, specification);

            return await query.FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<T>> GetListAsync(
            Specification<T> specification,
            CancellationToken token = default)
        {
            var dbSet = context.Set<T>();

            var query = EfSpecificationEvaluator.GetQuery(dbSet, specification);

            return await query.ToListAsync(token);
        }

        public async Task<int> CountAsync(Specification<T> specification, CancellationToken token = default)
        {
            var dbSet = context.Set<T>();

            var query = EfSpecificationEvaluator.GetQuery(dbSet, specification);

            return await query.CountAsync(token);
        }

        public async Task<bool> AnyAsync(
            Specification<T> specification,
            CancellationToken token = default)
        {
            var dbSet = context.Set<T>();

            var query = EfSpecificationEvaluator.GetQuery(dbSet, specification);

            return await query.AnyAsync(token);
        }

        public Task Add(T item, CancellationToken token = default)
        {
            context.Set<T>().Add(item);
            return Task.CompletedTask;
        }

        public Task Delete(T item, CancellationToken token = default)
        {
            context.Set<T>().Remove(item);
            return Task.CompletedTask;
        }

        public Task Update(T item, CancellationToken token = default)
        {
            context.Entry(item).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
