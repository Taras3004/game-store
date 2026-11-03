using GameStore.Models.Enums;

namespace GameStore.Core.Interfaces.Services
{
    public interface IUserProvider
    {
        bool IsAuthenticated { get; }
        Guid? GetUserId();
        bool HasPermission(Permission permission);
    }
}
