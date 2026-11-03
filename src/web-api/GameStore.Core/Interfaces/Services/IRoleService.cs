using GameStore.Core.Dto.Requests.Role;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<Result<RoleResponse>> CreateRole(CreateRoleRequest request, CancellationToken token = default);
        Task<Result<RoleResponse>> UpdateRole(UpdateRoleRequest request, CancellationToken token = default);

        Task<Result> DeleteRole(Guid id, CancellationToken token = default);
        Task<Result<RoleResponse>> GetRoleById(Guid id, CancellationToken token = default);
        Task<IEnumerable<RoleResponse>> GetAllRoles(CancellationToken token = default);
        IEnumerable<string> GetAllPermissions();
        Task<Result<IEnumerable<string>>> GetPermissionsByRoleId(Guid roleId, CancellationToken token = default);
        Task<Result<IEnumerable<RoleResponse>>> GetRolesByUserId(Guid userId, CancellationToken token = default);
    }
}
