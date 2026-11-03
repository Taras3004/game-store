using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using GameStore.Models.DomainEvents;

namespace GameStore.DAL.Data
{
    internal class GameStoreDbContextFactory : IDesignTimeDbContextFactory<GameStoreDbContext>
    {
        public GameStoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GameStoreDbContext>();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<GameStoreDbContextFactory>(optional: false)
                .Build();

            var connectionString = configuration["EFConnectionString"] ?? throw new InvalidOperationException();

            builder.UseSqlServer(connectionString);

            return new GameStoreDbContext(builder.Options, new NoOpDomainEventDispatcher());
        }

        private class NoOpDomainEventDispatcher : IDomainEventDispatcher
        {
            public Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }
    }
}