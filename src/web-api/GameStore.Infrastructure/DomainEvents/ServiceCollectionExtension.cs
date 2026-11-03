using GameStore.Models.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DomainEvents
{
    public static class ServiceCollectionExtension
    {
        public static void AddDomainEventDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        }
    }
}
