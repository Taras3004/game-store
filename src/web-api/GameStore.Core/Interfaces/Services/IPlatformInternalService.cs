using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Services
{
    internal interface IPlatformInternalService
    {
        Task<IEnumerable<Platform>> GetPlatformsByIds(IEnumerable<Guid> requestPlatforms, CancellationToken token);
    }
}