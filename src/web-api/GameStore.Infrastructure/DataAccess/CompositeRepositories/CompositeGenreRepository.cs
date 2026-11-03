using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DataAccess.CompositeRepositories
{
    internal class CompositeGenreRepository(
        [FromKeyedServices(DataSourceKeys.PrimaryDataSource)] IGenreRepository sqlRepo,
        [FromKeyedServices(DataSourceKeys.LegacyDataSource)] IGenreRepository mongoRepo) : IGenreRepository
    {
        public async Task AddAsync(Genre genre, CancellationToken token = default)
        {
            if (genre.ParentGenreId.HasValue)
            {
                await GetOrMigrateByIdsAsync([genre.ParentGenreId.Value], token);
            }

            await sqlRepo.AddAsync(genre, token);
        }

        public async Task UpdateAsync(Genre genre, CancellationToken token = default)
        {
            if (genre.ParentGenreId.HasValue)
            {
                await GetOrMigrateByIdsAsync([genre.ParentGenreId.Value], token);
            }

            var existingInSql = await sqlRepo.GetByIdAsync(genre.Id, token: token);

            if (existingInSql is null)
            {
                await sqlRepo.AddAsync(genre, token);
            }
            else
            {
                await sqlRepo.UpdateAsync(genre, token);
            }
        }

        public async Task DeleteAsync(Genre genre, CancellationToken token = default)
        {
            await sqlRepo.DeleteAsync(genre, token);
        }

        public async Task<Genre?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var sqlGenre = await sqlRepo.GetByIdAsync(id, tracking, token);
            if (sqlGenre is not null)
            {
                return sqlGenre;
            }

            var mongoGenre = await mongoRepo.GetByIdAsync(id, token: token);
            if (mongoGenre is null)
            {
                return null;
            }

            var sqlDuplicate = await sqlRepo.GetByNameAsync(mongoGenre.Name, tracking, token);
            return sqlDuplicate ?? mongoGenre;
        }

        public async Task<Genre?> GetByNameAsync(
            string name, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var sqlGenre = await sqlRepo.GetByNameAsync(name, tracking, token);
            if (sqlGenre is not null)
            {
                return sqlGenre;
            }

            return await mongoRepo.GetByNameAsync(name, token: token);
        }

        public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken token = default)
        {
            var sqlGenres = (await sqlRepo.GetAllAsync(token)).ToList();
            var mongoGenres = await mongoRepo.GetAllAsync(token);

            var sqlIds = sqlGenres
                .Select(g => g.Id)
                .ToHashSet();

            var sqlNames = sqlGenres
                .Select(g => g.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var uniqueMongoGenres = mongoGenres
                .Where(mg => !sqlIds.Contains(mg.Id) && !sqlNames.Contains(mg.Name));

            return sqlGenres.Concat(uniqueMongoGenres).ToList();
        }

        public async Task<IEnumerable<Genre>> GetByParentIdAsync(Guid parentId, CancellationToken token = default)
        {
            return await sqlRepo.GetByParentIdAsync(parentId, token);
        }

        public async Task<IEnumerable<Genre>> GetByGameKeyAsync(string gameKey, CancellationToken token = default)
        {
            var sqlGenres = (await sqlRepo.GetByGameKeyAsync(gameKey, token)).ToList();
            if (sqlGenres.Count != 0)
            {
                return sqlGenres;
            }

            var mongoGenres = (await mongoRepo.GetByGameKeyAsync(gameKey, token)).ToList();
            if (mongoGenres.Count == 0)
            {
                return [];
            }

            var result = new List<Genre>();
            foreach (var mongoGenre in mongoGenres)
            {
                var sqlGenre = await sqlRepo.GetByIdAsync(mongoGenre.Id, token: token)
                    ?? await sqlRepo.GetByNameAsync(mongoGenre.Name, token: token);

                result.Add(sqlGenre ?? mongoGenre);
            }

            return result;
        }

        public async Task<List<Genre>> GetOrMigrateByIdsAsync(
            IEnumerable<Guid> ids, CancellationToken token = default)
        {
            var idList = ids.Distinct().ToList();
            if (idList.Count == 0)
            {
                return [];
            }

            var resolvedGenres = await sqlRepo.GetOrMigrateByIdsAsync(idList, token);
            var foundIds = resolvedGenres.Select(g => g.Id).ToHashSet();

            var missingIds = idList.Where(id => !foundIds.Contains(id)).ToList();
            if (missingIds.Count == 0)
            {
                return resolvedGenres;
            }

            var categoriesToMigrate = await mongoRepo.GetOrMigrateByIdsAsync(missingIds, token);
            foreach (var category in categoriesToMigrate)
            {
                var sqlDuplicate = await sqlRepo.GetByNameAsync(category.Name, TrackingBehaviour.Tracking, token);
                if (sqlDuplicate is not null)
                {
                    if (resolvedGenres.All(g => g.Id != sqlDuplicate.Id))
                    {
                        resolvedGenres.Add(sqlDuplicate);
                    }

                    continue;
                }

                await sqlRepo.AddAsync(category, token);
                resolvedGenres.Add(category);
            }

            return resolvedGenres;
        }

        public async Task<bool> IsNameTakenAsync(
            string name, Guid? excludeId = null, CancellationToken token = default)
        {
            return await sqlRepo.IsNameTakenAsync(name, excludeId, token) ||
                   await mongoRepo.IsNameTakenAsync(name, excludeId, token);
        }
    }
}