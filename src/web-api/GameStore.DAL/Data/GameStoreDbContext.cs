using GameStore.Models.DomainEvents;
using GameStore.Models.DomainEvents.TransactionalOutbox;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Data
{
    internal class GameStoreDbContext(
        DbContextOptions<GameStoreDbContext> options,
        IDomainEventDispatcher domainEventDispatcher) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BanRecord> BanRecords { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameStoreDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await PublishDomainEventsAsync(cancellationToken);
                int savedEntries = await base.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return savedEntries;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task PublishDomainEventsAsync(CancellationToken token)
        {
            var domainEvents = ChangeTracker
                .Entries<IAggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(x =>
                {
                    List<IDomainEvent> domainEvents = x.DomainEvents.ToList();

                    x.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();

            await domainEventDispatcher.DispatchAsync(domainEvents, token);
        }
    }
}
