using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DataAccess.CompositeRepositories
{
    internal class CompositeGameRepository(
        [FromKeyedServices(DataSourceKeys.PrimaryDataSource)] IGameRepository sqlRepo,
        [FromKeyedServices(DataSourceKeys.LegacyDataSource)] IGameRepository mongoRepo,
        IPublisherRepository publisherRepo,
        IGenreRepository genreRepo) : IGameRepository
    {
        private static readonly GamePermissionDetails FullAccess = new()
        {
            AccessDeleted = true,
            AccessOutOfStock = true,
        };

        public async Task AddAsync(Game game, CancellationToken token = default)
        {
            await sqlRepo.AddAsync(game, token);
        }

        public async Task UpdateAsync(Game game, CancellationToken token = default)
        {
            // The existence check is not tracked so it can't conflict with the instance being updated
            var existingInSql = await sqlRepo.GetByIdAsync(game.Id, permissions: null, token: token);
            if (existingInSql is not null)
            {
                await sqlRepo.UpdateAsync(game, token);
                return;
            }

            // E08 US7: Mongo data is never modified, an edited product is copied into SQL
            if (game.GameGenres.Count == 0)
            {
                var original = await mongoRepo.GetByIdAsync(game.Id, permissions: null, token: token);
                if (original is not null)
                {
                    var genres = await genreRepo.GetByGameKeyAsync(original.Key, token);
                    var migratedGenres = await genreRepo.GetOrMigrateByIdsAsync(genres.Select(g => g.Id), token);
                    game.AddGenreRange(migratedGenres);
                }
            }

            await sqlRepo.AddAsync(game, token);
        }

        public async Task DeleteAsync(Game game, CancellationToken token = default)
        {
            await sqlRepo.DeleteAsync(game, token);
        }

        public async Task<Game?> GetByIdAsync(
            Guid id,
            GamePermissionDetails? permissions = null,
            TrackingBehaviour tracking = TrackingBehaviour.NoTracking,
            CancellationToken token = default)
        {
            var rawSqlGame = await sqlRepo.GetByIdAsync(id, permissions: null, token: token);
            if (rawSqlGame is not null)
            {
                return await sqlRepo.GetByIdAsync(id, permissions, tracking, token);
            }

            return await mongoRepo.GetByIdAsync(id, permissions, token: token);
        }

        public async Task<Game?> GetByKeyAsync(
            string key,
            GamePermissionDetails? permissions = null,
            TrackingBehaviour tracking = TrackingBehaviour.NoTracking,
            CancellationToken token = default)
        {
            var rawSqlGame = await sqlRepo.GetByKeyAsync(key, permissions: null, token: token);
            if (rawSqlGame is not null)
            {
                return await sqlRepo.GetByKeyAsync(key, permissions, tracking, token);
            }

            var mongoGame = await mongoRepo.GetByKeyAsync(key, permissions, token: token);
            if (mongoGame is null)
            {
                return null;
            }

            var migratedById = await sqlRepo.GetByIdAsync(mongoGame.Id, permissions: null, token: token);
            return migratedById is not null ? null : mongoGame;
        }

        public async Task<IEnumerable<Game>> GetByFilterAsync(
            GameFilterDetails filter, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var sqlGames = await sqlRepo.GetByFilterAsync(filter, permissions, token);
            var mongoGames = await mongoRepo.GetByFilterAsync(filter, permissions, token);

            return await MergeGamesAsync(sqlGames, mongoGames, token);
        }

        public async Task<IEnumerable<Game>> GetByPlatformIdAsync(
            Guid platformId, GamePermissionDetails permissions, CancellationToken token = default)
        {
            return await sqlRepo.GetByPlatformIdAsync(platformId, permissions, token);
        }

        public async Task<IEnumerable<Game>> GetByGenreIdAsync(
            Guid genreId, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var sqlGames = await sqlRepo.GetByGenreIdAsync(genreId, permissions, token);
            var mongoGames = await mongoRepo.GetByGenreIdAsync(genreId, permissions, token);

            return await MergeGamesAsync(sqlGames, mongoGames, token);
        }

        public async Task<IEnumerable<Game>> GetByPublisherCompanyNameAsync(
            string companyName, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var sqlGames = await sqlRepo.GetByPublisherCompanyNameAsync(companyName, permissions, token);
            var mongoGames = await mongoRepo.GetByPublisherCompanyNameAsync(companyName, permissions, token);

            return await MergeGamesAsync(sqlGames, mongoGames, token);
        }

        public async Task<int> GetCountAsync(GameFilterDetails? filter = null, GamePermissionDetails? permissions = null, CancellationToken token = default)
        {
            // Counted from the merged result so duplicates and stale Mongo copies are excluded the same way as in lists
            var games = await GetByFilterAsync(filter ?? new GameFilterDetails(), permissions ?? FullAccess, token);

            return games.Count();
        }

        public async Task<bool> IsKeyTakenAsync(
            string key, Guid? excludeId = null, CancellationToken token = default)
        {
            return await sqlRepo.IsKeyTakenAsync(key, excludeId, token) ||
                   await mongoRepo.IsKeyTakenAsync(key, excludeId, token);
        }

        public async Task<Game?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default)
        {
            var sqlGame = await sqlRepo.GetOrMigrateByIdAsync(id, token);
            if (sqlGame is not null)
            {
                return sqlGame;
            }

            var mongoGame = await mongoRepo.GetByIdAsync(id, permissions: null, token: token);
            if (mongoGame is null)
            {
                return null;
            }

            return await MigrateMongoGameToSqlAsync(mongoGame, token);
        }

        public async Task<Game?> GetOrMigrateByKeyAsync(string key, CancellationToken token = default)
        {
            var sqlGame = await sqlRepo.GetOrMigrateByKeyAsync(key, token);
            if (sqlGame is not null)
            {
                return sqlGame;
            }

            var mongoGame = await mongoRepo.GetByKeyAsync(key, permissions: null, token: token);
            if (mongoGame is null)
            {
                return null;
            }

            return await MigrateMongoGameToSqlAsync(mongoGame, token);
        }

        public async Task IncrementViewsByIdAsync(Guid id, CancellationToken token = default)
        {
            if (await sqlRepo.GetByIdAsync(id, permissions: null, token: token) is not null)
            {
                await sqlRepo.IncrementViewsByIdAsync(id, token);
                return;
            }

            await mongoRepo.IncrementViewsByIdAsync(id, token);
        }

        public async Task IncrementViewsByKeyAsync(string key, CancellationToken token = default)
        {
            if (await sqlRepo.IsKeyTakenAsync(key, excludeId: null, token))
            {
                await sqlRepo.IncrementViewsByKeyAsync(key, token);
                return;
            }

            await mongoRepo.IncrementViewsByKeyAsync(key, token);
        }

        public async Task IncrementCommentCountAsync(string key, CancellationToken token = default)
        {
            await sqlRepo.IncrementCommentCountAsync(key, token);
        }

        public async Task SyncStockAsync(string key, int newStock, CancellationToken token = default)
        {
            await sqlRepo.SyncStockAsync(key, newStock, token);
            await mongoRepo.SyncStockAsync(key, newStock, token);
        }

        private async Task<Game?> MigrateMongoGameToSqlAsync(Game mongoGame, CancellationToken token)
        {
            var publisher = await publisherRepo.GetOrMigrateByIdAsync(mongoGame.PublisherId, token);
            if (publisher is null)
            {
                return null;
            }

            if (publisher.Id != mongoGame.PublisherId)
            {
                mongoGame.UpdateDetails(
                    mongoGame.Name,
                    mongoGame.Key,
                    mongoGame.Description,
                    mongoGame.GamePrice,
                    mongoGame.UnitInStock,
                    publisher.Id);
            }

            var genres = await genreRepo.GetByGameKeyAsync(mongoGame.Key, token);
            var genreIds = genres.Select(g => g.Id).ToList();
            if (genreIds.Count != 0)
            {
                var migratedGenres = await genreRepo.GetOrMigrateByIdsAsync(genreIds, token);
                mongoGame.AddGenreRange(migratedGenres);
            }

            await sqlRepo.AddAsync(mongoGame, token);
            return mongoGame;
        }

        /// <summary>
        /// E08 US6/US7: a Mongo product is hidden when SQL holds any game with the same id (a migrated copy)
        /// or the same key, even if that SQL game was excluded by the current filter or permissions.
        /// </summary>
        private async Task<List<Game>> MergeGamesAsync(
            IEnumerable<Game> sqlGames, IEnumerable<Game> mongoGames, CancellationToken token)
        {
            var mongoGameList = mongoGames.ToList();
            if (mongoGameList.Count == 0)
            {
                return sqlGames.ToList();
            }

            var allSqlGames = (await sqlRepo.GetByFilterAsync(new GameFilterDetails(), FullAccess, token)).ToList();

            var sqlIds = allSqlGames
                .Select(g => g.Id)
                .ToHashSet();

            var sqlKeys = allSqlGames
                .Select(g => g.Key)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var uniqueMongoGames = mongoGameList
                .Where(mg => !sqlIds.Contains(mg.Id) && !sqlKeys.Contains(mg.Key));

            return sqlGames.Concat(uniqueMongoGames).ToList();
        }
    }
}