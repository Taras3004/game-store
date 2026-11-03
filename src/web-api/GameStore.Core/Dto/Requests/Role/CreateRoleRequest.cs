using GameStore.Core.Dto.Details.Role;
using GameStore.Models.Enums;

namespace GameStore.Core.Dto.Requests.Role
{
    public record CreateRoleRequest
    {
        public required RoleDetailsBase Role { get; init; }
        public required IEnumerable<Permission> Permissions { get; init; }
    }
}
