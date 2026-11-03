using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering.Contracts;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Data;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    internal class GameRepository(
        GameStoreDbContext context,
        IEntityFilter<Game, GameFilterDetails> gameFilter) : IGameRepository
    {
        private readonly DbSet<Game> games = context.Set<Game>();

        public Task AddAsync(Game game, CancellationToken token = default)
        {
            games.Add(game);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Game game, CancellationToken token = default)
        {
            if (context.Entry(game).State == EntityState.Detached)
            {
                games.Update(game);
            }

            return Task.CompletedTask;
        }

        public async Task<Game?> GetByIdAsync(
            Guid id,
            GamePermissionDetails? permissions = null,
            TrackingBehaviour tracking = TrackingBehaviour.NoTracking,
            CancellationToken token = default)
        {
            var query = ApplyPermissions(games.ApplyTracking(tracking), permissions);

            return await query
                .Include(g => g.GameGenres)
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g => g.Id == id, token);
        }

        public async Task<Game?> GetByKeyAsync(
            string key,
            GamePermissionDetails? permissions = null,
            TrackingBehaviour tracking = TrackingBehaviour.NoTracking,
            CancellationToken token = default)
        {
            var query = ApplyPermissions(games.ApplyTracking(tracking), permissions);

            return await query
                .Include(g => g.GameGenres)
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g => g.Key == key, token);
        }

        public async Task<IEnumerable<Game>> GetByFilterAsync(
            GameFilterDetails filter, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var predicate = await gameFilter.CreateFilter(filter);
            var query = ApplyPermissions(games.AsNoTracking(), permissions);

            return await query
                .Where(predicate)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Game>> GetByPlatformIdAsync(
            Guid platformId, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var query = ApplyPermissions(games.AsNoTracking(), permissions);

            return await query
                .Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == platformId))
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Game>> GetByGenreIdAsync(
            Guid genreId, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var query = ApplyPermissions(games.AsNoTracking(), permissions);

            return await query
                .Where(g => g.GameGenres.Any(gg => gg.GenreId == genreId))
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Game>> GetByPublisherCompanyNameAsync(
            string companyName, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var query = ApplyPermissions(games.AsNoTracking(), permissions);

            return await query
                .Where(g => g.Publisher.CompanyName == companyName)
                .ToListAsync(token);
        }

        public async Task<int> GetCountAsync(
            GameFilterDetails? filter = null,
            GamePermissionDetails? permissions = null,
            CancellationToken token = default)
        {
            var query = ApplyPermissions(games.AsNoTracking(), permissions)
                .Where(g => g.LegacyId == null);

            if (filter is not null)
            {
                var predicate = await gameFilter.CreateFilter(filter);
                query = query.Where(predicate);
            }

            return await query.CountAsync(token);
        }

        public async Task<bool> IsKeyTakenAsync(
            string key, Guid? excludeId = null, CancellationToken token = default)
        {
            var query = games
                .AsNoTracking()
                .Where(g => g.Key == key);

            if (excludeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeId.Value);
            }

            return await query.AnyAsync(token);
        }

        public Task<Game?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default)
        {
            return GetByIdAsync(id, permissions: null, TrackingBehaviour.Tracking, token);
        }

        public Task<Game?> GetOrMigrateByKeyAsync(string key, CancellationToken token = default)
        {
            return GetByKeyAsync(key, permissions: null, TrackingBehaviour.Tracking, token);
        }

        public async Task IncrementViewsByIdAsync(Guid id, CancellationToken token = default)
        {
            await games
                .Where(g => g.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(g => g.Views, g => g.Views + 1), token);
        }

        public async Task IncrementViewsByKeyAsync(string key, CancellationToken token = default)
        {
            await games
                .Where(g => g.Key == key)
                .ExecuteUpdateAsync(s => s.SetProperty(g => g.Views, g => g.Views + 1), token);
        }

        public async Task IncrementCommentCountAsync(string key, CancellationToken token = default)
        {
            await games
                .Where(g => g.Key == key)
                .ExecuteUpdateAsync(s => s.SetProperty(g => g.CommentsCount, g => g.CommentsCount + 1), token);
        }

        public async Task SyncStockAsync(string key, int newStock, CancellationToken token = default)
        {
            await games
                .Where(g => g.Key == key)
                .ExecuteUpdateAsync(s => s.SetProperty(g => g.UnitInStock, newStock), token);
        }

        public Task DeleteAsync(Game game, CancellationToken token = default)
        {
            games.Remove(game);
            return Task.CompletedTask;
        }

        private static IQueryable<Game> ApplyPermissions(
            IQueryable<Game> query, GamePermissionDetails? permissions)
        {
            if (permissions is null)
            {
                return query;
            }

            if (!permissions.AccessDeleted)
            {
                query = query.Where(g => !g.IsDeleted);
            }

            if (!permissions.AccessOutOfStock)
            {
                query = query.Where(g => g.UnitInStock > 0);
            }

            return query;
        }
    }
}