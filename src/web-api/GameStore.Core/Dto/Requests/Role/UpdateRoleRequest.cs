using GameStore.Core.Dto.Details.Role;
using GameStore.Models.Enums;

namespace GameStore.Core.Dto.Requests.Role
{
    public record UpdateRoleRequest
    {
        public required RoleDetailsWithId Role { get; init; }
        public required IEnumerable<Permission> Permissions { get; init; }
    }
}
