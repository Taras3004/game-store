using GameStore.Models.Constants.ValidationValues;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.Web.Infrastructure.Authorization.Permissions
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userClaims = context.User.Claims;

            var permissionClaim = userClaims
                .FirstOrDefault(x => x.Value.EndsWith(requirement.Permission.ToString()) && 
                                                                     x.Type == UserValidation.PermissionPrefix);

            if (permissionClaim is not null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
