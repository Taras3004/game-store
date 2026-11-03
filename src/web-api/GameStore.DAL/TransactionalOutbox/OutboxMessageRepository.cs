using GameStore.DAL.Data;
using GameStore.Models.DomainEvents.TransactionalOutbox;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.TransactionalOutbox
{
    internal class OutboxMessageRepository(GameStoreDbContext context) : IOutboxMessageRepository
    {
        public Task Add(OutboxMessage item, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OutboxMessage>> GetUncompletedMessagesChunk(int messagesCount, CancellationToken token = default)
        {
            return await context.OutboxMessages
                .Where(x => x.CompletedAt == null && x.RetryCount < 3)
                .OrderBy(x => x.CreatedDate)
                .Take(messagesCount)
                .ToListAsync(token);
        }

        public Task Update(OutboxMessage item, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
