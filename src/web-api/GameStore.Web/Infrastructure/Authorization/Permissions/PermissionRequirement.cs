using GameStore.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.Web.Infrastructure.Authorization.Permissions
{
    public class PermissionRequirement(Permission permission) : IAuthorizationRequirement
    {
        public Permission Permission { get; } = permission;
    }
}
