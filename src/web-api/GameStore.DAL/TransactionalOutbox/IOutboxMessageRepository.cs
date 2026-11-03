using GameStore.Models.DomainEvents.TransactionalOutbox;

namespace GameStore.DAL.TransactionalOutbox
{
    public interface IOutboxMessageRepository
    {
        Task Add(OutboxMessage item, CancellationToken token = default);
        Task Update(OutboxMessage item, CancellationToken token = default);
        Task<List<OutboxMessage>> GetUncompletedMessagesChunk(int messagesCount, CancellationToken token = default);
        //Task<TEntity?> GetFirstOrDefaultAsync(Specification<TEntity> specification, CancellationToken token = default);
        //Task<IEnumerable<TEntity>> GetListAsync(Specification<TEntity> specification, CancellationToken token = default);
        //Task<int> CountAsync(Specification<TEntity> specification, CancellationToken token = default);
        //Task<bool> AnyAsync(Specification<TEntity> specification, CancellationToken token = default);
    }
}
