using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class UserAccessChecker(IUserService userService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await userService.GetUserById(targetId, token);
            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
