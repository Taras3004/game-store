using System.Text.Json;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.DomainEvents.TransactionalOutbox;
using GameStore.Models.DomainEvents.TransactionalOutbox.MessageTypes;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GameStore.DAL.Data.Interceptors
{
    internal class OutboxMessageSaveChangesInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new())
        {
            var context = eventData.Context as GameStoreDbContext;

            ArgumentNullException.ThrowIfNull(context);

            var entries = context.ChangeTracker
                .Entries()
                .Where(x => x.Entity is not OutboxMessage &&
                            (x.State == EntityState.Modified || 
                            x.State == EntityState.Added ||
                            x.State == EntityState.Deleted))
                .ToList();

            foreach (var entry in entries)
            {
                DateTime utcNow = dateTimeProvider.UtcNow;

                if (entry is { State: EntityState.Modified, Entity: Game { LegacyId: not null } game } &&
                    entry.Property(nameof(Game.UnitInStock)).IsModified)
                {
                    SyncStockOutboxMessage syncMessage = new SyncStockOutboxMessage(game.Key, game.UnitInStock);

                    string serializedData = JsonSerializer.Serialize(syncMessage);

                    OutboxMessage outboxMessage = new OutboxMessage(utcNow, typeof(SyncStockOutboxMessage).FullName!, serializedData);
                    context.OutboxMessages.Add(outboxMessage);
                }

                string action = entry.State switch
                {
                    EntityState.Added => "Create",
                    EntityState.Modified => "Update",
                    EntityState.Deleted => "Delete",
                    _ => throw new ArgumentException("Unknown Entity State"),
                };

                object? oldState = entry.State == EntityState.Added ? null : entry.OriginalValues.ToObject();
                object? newState = entry.State == EntityState.Deleted ? null : entry.CurrentValues.ToObject();

                LogOutboxMessage logMessage = new LogOutboxMessage(entry.Entity.GetType().Name,
                    action,
                    oldState != null ? JsonSerializer.Serialize(oldState) : string.Empty,
                    newState != null ? JsonSerializer.Serialize(newState) : string.Empty,
                    utcNow);

                string serializedLog = JsonSerializer.Serialize(logMessage);

                OutboxMessage logOutbox = new(utcNow, typeof(LogOutboxMessage).FullName!, serializedLog);
                context.OutboxMessages.Add(logOutbox);
            }
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
