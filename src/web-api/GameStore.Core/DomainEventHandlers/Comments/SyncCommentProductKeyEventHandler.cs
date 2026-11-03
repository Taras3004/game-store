using GameStore.Core.Interfaces;
using GameStore.Models.DomainEvents;
using GameStore.Models.DomainEvents.Events.Games;

namespace GameStore.Core.DomainEventHandlers.Comments
{
    internal class SyncCommentProductKeyEventHandler(IUnitOfWork unitOfWork)
        : IDomainEventHandler<GameKeyUpdatedEvent>
    {
        public async Task Handle(GameKeyUpdatedEvent domainEvent, CancellationToken cancellationToken)
        {
            await unitOfWork.Comments
                .BatchProductKeyUpdate(domainEvent.OldKey, domainEvent.NewKey, cancellationToken);
        }
    }
}
