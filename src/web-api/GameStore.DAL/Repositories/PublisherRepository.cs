using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Data;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    internal class PublisherRepository(GameStoreDbContext context) : IPublisherRepository
    {
        private readonly DbSet<Publisher> publishers = context.Set<Publisher>();

        public Task AddAsync(Publisher publisher, CancellationToken token = default)
        {
            publishers.Add(publisher);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Publisher publisher, CancellationToken token = default)
        {
            if (context.Entry(publisher).State == EntityState.Detached)
            {
                publishers.Update(publisher);
            }

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            var publisher = await publishers.FirstOrDefaultAsync(p => p.Id == id, token);
            if (publisher is not null)
            {
                publishers.Remove(publisher);
            }
        }

        public Task DeleteAsync(Publisher publisher, CancellationToken token = default)
        {
            publishers.Remove(publisher);
            return Task.CompletedTask;
        }

        public async Task<Publisher?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            return await publishers
                .ApplyTracking(tracking)
                .FirstOrDefaultAsync(p => p.Id == id, token);
        }

        public async Task<Publisher?> GetByCompanyNameAsync(
            string companyName, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            return await publishers
                .ApplyTracking(tracking)
                .FirstOrDefaultAsync(p => p.CompanyName == companyName, token);
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync(CancellationToken token = default)
        {
            return await publishers
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<Publisher?> GetByGameKeyAsync(
            string gameKey, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            return await publishers
                .ApplyTracking(tracking)
                .FirstOrDefaultAsync(p => p.Games.Any(g => g.Key == gameKey), token);
        }

        public async Task<Publisher?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default)
        {
            return await publishers
                .FirstOrDefaultAsync(p => p.Id == id, token);
        }

        public async Task<bool> IsNameTakenAsync(string companyName, CancellationToken token = default)
        {
            return await publishers
                .AsNoTracking()
                .AnyAsync(p => p.CompanyName == companyName, token);
        }

        public async Task<bool> IsNameTakenExcludeIdAsync(string companyName, Guid excludeId, CancellationToken token = default)
        {
            return await publishers
                .AsNoTracking()
                .AnyAsync(p => p.CompanyName == companyName && p.Id != excludeId, token);
        }
    }
}