using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DataAccess.CompositeRepositories
{
    internal class CompositePublisherRepository(
        [FromKeyedServices(DataSourceKeys.PrimaryDataSource)] IPublisherRepository sqlRepo,
        [FromKeyedServices(DataSourceKeys.LegacyDataSource)] IPublisherRepository mongoRepo) : IPublisherRepository
    {
        public async Task AddAsync(Publisher publisher, CancellationToken token = default)
        {
            await sqlRepo.AddAsync(publisher, token);
        }

        public async Task UpdateAsync(Publisher publisher, CancellationToken token = default)
        {
            var existingInSql = await sqlRepo.GetByIdAsync(publisher.Id, token: token);

            if (existingInSql is null)
            {
                await sqlRepo.AddAsync(publisher, token);
            }
            else
            {
                await sqlRepo.UpdateAsync(publisher, token);
            }
        }

        public async Task DeleteAsync(Publisher publisher, CancellationToken token = default)
        {
            await sqlRepo.DeleteAsync(publisher, token);
        }

        public async Task<Publisher?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var sqlPublisher = await sqlRepo.GetByIdAsync(id, tracking, token);
            if (sqlPublisher is not null)
            {
                return sqlPublisher;
            }

            var mongoPublisher = await mongoRepo.GetByIdAsync(id, token: token);
            return await PreferSqlDuplicateAsync(mongoPublisher, tracking, token);
        }

        public async Task<Publisher?> GetByCompanyNameAsync(
            string companyName, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var sqlPublisher = await sqlRepo.GetByCompanyNameAsync(companyName, tracking, token);
            if (sqlPublisher is not null)
            {
                return sqlPublisher;
            }

            return await mongoRepo.GetByCompanyNameAsync(companyName, token: token);
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync(CancellationToken token = default)
        {
            var sqlPublishers = (await sqlRepo.GetAllAsync(token)).ToList();
            var mongoPublishers = await mongoRepo.GetAllAsync(token);

            var sqlIds = sqlPublishers
                .Select(p => p.Id)
                .ToHashSet();

            var sqlNames = sqlPublishers
                .Select(p => p.CompanyName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var uniqueMongoPublishers = mongoPublishers
                .Where(mp => !sqlIds.Contains(mp.Id) && !sqlNames.Contains(mp.CompanyName));

            return sqlPublishers.Concat(uniqueMongoPublishers).ToList();
        }

        public async Task<Publisher?> GetByGameKeyAsync(
            string gameKey, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var sqlPublisher = await sqlRepo.GetByGameKeyAsync(gameKey, tracking, token);
            if (sqlPublisher is not null)
            {
                return sqlPublisher;
            }

            var mongoPublisher = await mongoRepo.GetByGameKeyAsync(gameKey, token: token);
            return await PreferSqlDuplicateAsync(mongoPublisher, tracking, token);
        }

        public async Task<Publisher?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default)
        {
            var sqlPublisher = await sqlRepo.GetByIdAsync(id, TrackingBehaviour.Tracking, token);
            if (sqlPublisher is not null)
            {
                return sqlPublisher;
            }

            var mongoPublisher = await mongoRepo.GetByIdAsync(id, token: token);
            if (mongoPublisher is null)
            {
                return null;
            }

            var sqlDuplicate = await sqlRepo.GetByCompanyNameAsync(mongoPublisher.CompanyName, TrackingBehaviour.Tracking, token);
            if (sqlDuplicate is not null)
            {
                return sqlDuplicate;
            }

            await sqlRepo.AddAsync(mongoPublisher, token);
            return mongoPublisher;
        }

        public async Task<bool> IsNameTakenAsync(string companyName, CancellationToken token = default)
        {
            return await sqlRepo.IsNameTakenAsync(companyName, token) ||
                   await mongoRepo.IsNameTakenAsync(companyName, token);
        }

        public async Task<bool> IsNameTakenExcludeIdAsync(string companyName, Guid excludeId, CancellationToken token = default)
        {
            return await sqlRepo.IsNameTakenExcludeIdAsync(companyName, excludeId, token) ||
                   await mongoRepo.IsNameTakenExcludeIdAsync(companyName, excludeId, token);
        }

        private async Task<Publisher?> PreferSqlDuplicateAsync(
            Publisher? mongoPublisher, TrackingBehaviour tracking, CancellationToken token)
        {
            if (mongoPublisher is null)
            {
                return null;
            }

            var sqlDuplicate = await sqlRepo.GetByCompanyNameAsync(mongoPublisher.CompanyName, tracking, token);
            return sqlDuplicate ?? mongoPublisher;
        }
    }
}
