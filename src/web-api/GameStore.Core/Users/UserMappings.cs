using System.Collections.Frozen;
using System.Security.Claims;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Enums;

namespace GameStore.Core.Users
{
    internal static class UserMappings
    {
        public static IReadOnlyDictionary<string, TimeSpan> BanDurationMap { get; } = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
        {
            {"1 hour", TimeSpan.FromHours(1)},
            {"1 day", TimeSpan.FromDays(1)},
            {"1 week", TimeSpan.FromDays(7)},
            {"1 month", TimeSpan.FromDays(30)},
            {"permanent", TimeSpan.MaxValue}
        }.ToFrozenDictionary();

        public static List<Claim> PermissionsToClaims(IEnumerable<Permission> permissions)
        {
            List<Claim> claims = new List<Claim>();

            foreach (Permission permissionType in permissions)
            {
                claims.Add(new Claim(UserValidation.PermissionPrefix, permissionType.ToString()));
            }

            return claims;
        }
    }
}
