using GameStore.Core.Interfaces.Services;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Enums;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GameStore.Web.Infrastructure
{
    internal class UserProvider(IHttpContextAccessor accessor) : IUserProvider
    {
        public bool IsAuthenticated =>
            accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

        public Guid? GetUserId()
        {
            var idClaim = accessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (idClaim is null)
                return null;

            if (Guid.TryParse(idClaim.Value, out Guid userId))
            {
                return userId;
            }

            return null;
        }

        public bool HasPermission(Permission permission) => 
            accessor.HttpContext?.User.Claims
                .Any(c => c.Type == UserValidation.PermissionPrefix && c.Value == permission.ToString()) ?? false;
    }
}
