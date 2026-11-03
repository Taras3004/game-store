using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Repositories;
using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Repositories.Logger;
using GameStore.DAL.TransactionalOutbox;
using GameStore.Infrastructure.DataAccess;
using GameStore.Models.DomainEvents.TransactionalOutbox.MessageTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Text.Json;

namespace GameStore.DAL.BackgroundServices
{
    internal class OutboxProcessorBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessorBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessOutboxMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Critical error in OutboxProcessor");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessOutboxMessagesAsync(CancellationToken token)
        {
            using var scope = scopeFactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var outboxMessageRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();

            var logRepository = scope.ServiceProvider.GetRequiredService<ILogRepository>();
            var mongoGameRepository = scope.ServiceProvider.GetRequiredKeyedService<IGameRepository>(DataSourceKeys.LegacyDataSource);

            int chunkSize = 50;
            var messages = await outboxMessageRepository.GetUncompletedMessagesChunk(chunkSize, token);

            if (messages.Count == 0)
            {
                return;
            }

            foreach (var message in messages)
            {
                try
                {
                    if (message.Type == typeof(LogOutboxMessage).FullName)
                    {
                        var logMessage = JsonSerializer.Deserialize<LogOutboxMessage>(message.Payload);
                        if (logMessage is not null)
                        {
                            Log log = new Log()
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                Date = logMessage.CreatedDate,
                                Action = logMessage.Action,
                                EntityType = logMessage.EntityName,
                                OldVersion = logMessage.OldVersion,
                                NewVersion = logMessage.NewVersion
                            };
                            await logRepository.Add(log, token);
                        }
                    }
                    else if (message.Type == typeof(SyncStockOutboxMessage).FullName)
                    {
                        var syncMessage = JsonSerializer.Deserialize<SyncStockOutboxMessage>(message.Payload);
                        if (syncMessage is not null)
                        {
                            await mongoGameRepository.SyncStockAsync(syncMessage.GameKey, syncMessage.UnitInStockValue, token);
                        }
                    }
                    else
                    {
                        logger.LogWarning("Unknown outbox message type: {Type}", message.Type);
                    }

                    message.Complete(DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                    message.SetError(ex.ToString());
                }
            }

            await unitOfWork.SaveChangesAsync(token);
        }
    }
}
