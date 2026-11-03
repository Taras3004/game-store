using System.Reflection;
using GameStore.Models.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DomainEvents
{
    internal class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
    {
        public async Task DispatchAsync(
            IEnumerable<IDomainEvent> domainEvents,
            CancellationToken token = default)
        {
            foreach (IDomainEvent domainEvent in domainEvents)
            {
                Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                IEnumerable<object?> handlers = serviceProvider.GetServices(handlerType);

                foreach (object? handler in handlers)
                {
                    if (handler is null)
                    {
                        continue;
                    }

                    MethodInfo? handleMethod = handlerType.GetMethod("Handle");

                    if (handleMethod is not null)
                    {
                        await (Task)handleMethod.Invoke(handler, [domainEvent, token])!;
                    }
                }
            }
        }
    }
}
