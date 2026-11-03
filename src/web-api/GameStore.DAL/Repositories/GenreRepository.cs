using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Data;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    internal class GenreRepository(GameStoreDbContext context) : IGenreRepository
    {
        private readonly DbSet<Genre> _genres = context.Set<Genre>();

        public Task AddAsync(Genre genre, CancellationToken token = default)
        {
            _genres.Add(genre);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Genre genre, CancellationToken token = default)
        {
            _genres.Remove(genre);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Genre genre, CancellationToken token = default)
        {
            if (context.Entry(genre).State == EntityState.Detached)
            {
                _genres.Update(genre);
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken token = default)
        {
            return await _genres
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<Genre?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            return await _genres
                .ApplyTracking(tracking)
                .FirstOrDefaultAsync(g => g.Id == id, token);
        }

        public async Task<Genre?> GetByNameAsync(
            string name, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            return await _genres
                .ApplyTracking(tracking)
                .FirstOrDefaultAsync(g => g.Name == name, token);
        }

        public async Task<IEnumerable<Genre>> GetByParentIdAsync(Guid parentId, CancellationToken token = default)
        {
            return await _genres
                .AsNoTracking()
                .Where(g => g.ParentGenreId == parentId)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Genre>> GetByGameKeyAsync(string gameKey, CancellationToken token = default)
        {
            return await _genres
                .AsNoTracking()
                .Where(g => g.GameGenres.Any(gg => gg.Game.Key == gameKey))
                .ToListAsync(token);
        }

        public async Task<List<Genre>> GetOrMigrateByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default)
        {
            var idSet = ids.ToHashSet();
            if (idSet.Count == 0)
            {
                return [];
            }

            return await _genres
                .Where(g => idSet.Contains(g.Id))
                .ToListAsync(token);
        }

        public async Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null, CancellationToken token = default)
        {
            var query = _genres
                .AsNoTracking()
                .Where(g => g.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeId.Value);
            }

            return await query.AnyAsync(token);
        }
    }
}