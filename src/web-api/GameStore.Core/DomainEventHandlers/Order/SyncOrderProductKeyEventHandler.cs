using GameStore.Core.Interfaces;
using GameStore.Models.DomainEvents;
using GameStore.Models.DomainEvents.Events.Games;

namespace GameStore.Core.DomainEventHandlers.Order
{
    internal class SyncOrderProductKeyEventHandler(IUnitOfWork unitOfWork) 
        : IDomainEventHandler<GameKeyUpdatedEvent>
    {
        public async Task Handle(GameKeyUpdatedEvent domainEvent, CancellationToken cancellationToken)
        {
            await unitOfWork.Orders
                .BatchProductKeyUpdateAsync(domainEvent.OldKey, domainEvent.NewKey, cancellationToken);
        }
    }
}
