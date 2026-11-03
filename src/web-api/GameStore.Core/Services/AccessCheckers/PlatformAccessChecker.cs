using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class PlatformAccessChecker(IPlatformService platformService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await platformService.GetPlatform(targetId, token);
            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
