using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Interfaces
{
    public interface IEntityAccessCheckerFactory
    {
        IEntityAccessChecker? GetAccessChecker(string targetEntity);
    }
}
