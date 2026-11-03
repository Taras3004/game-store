using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class RoleAccessChecker(IRoleService roleService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await roleService.GetRoleById(targetId, token);
            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
