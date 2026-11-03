using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace GameStore.Web.Infrastructure.Authorization.Permissions
{
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        : DefaultAuthorizationPolicyProvider(options)
    {
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(UserValidation.PermissionPrefix, StringComparison.OrdinalIgnoreCase) &&
                Enum.TryParse(policyName.Substring(UserValidation.PermissionPrefix.Length), out Permission permission))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(permission));
                return policy.Build();
            }

            return await base.GetPolicyAsync(policyName);
        }
    }
}
