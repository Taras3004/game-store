using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.Web.Infrastructure.Authorization.Permissions
{
    public class NeedsPermissionAttribute(Permission permission)
        : AuthorizeAttribute(policy: $"{UserValidation.PermissionPrefix}{permission.ToString()}");
}
