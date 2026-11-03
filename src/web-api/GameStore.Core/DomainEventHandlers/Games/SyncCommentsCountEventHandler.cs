using GameStore.Core.Interfaces.Repositories;
using GameStore.Models.DomainEvents;
using GameStore.Models.DomainEvents.Events.Comments;

namespace GameStore.Core.DomainEventHandlers.Games
{
    internal class SyncCommentsCountEventHandler(IGameRepository gameSource)
        : IDomainEventHandler<CommentCreatedEvent>
    {
        public async Task Handle(CommentCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            await gameSource.IncrementCommentCountAsync(domainEvent.GameKey, cancellationToken);
        }
    }
}
