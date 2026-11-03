using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class EntityAccessCheckerFactory(IServiceProvider serviceProvider) : IEntityAccessCheckerFactory
    {
        public IEntityAccessChecker? GetAccessChecker(string targetEntity)
        {
            return serviceProvider.GetKeyedService<IEntityAccessChecker>(targetEntity);
        }
    }
}
