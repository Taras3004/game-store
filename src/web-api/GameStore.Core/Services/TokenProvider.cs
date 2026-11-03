using System.Security.Claims;
using System.Text;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Users;
using GameStore.Core.Users.Options;
using GameStore.Models.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace GameStore.Core.Services
{
    internal class TokenProvider(
        IOptions<JwtOptions> jwtOptionsSnapshot,
        IDateTimeProvider dateTimeProvider): ITokenProvider
    {

    public string CreateToken(User user)
    {
        var optionsValue = jwtOptionsSnapshot.Value;
        string secret = optionsValue.Secret;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var utcNow = dateTimeProvider.UtcNow;

        var userPermissionClaims = UserMappings.PermissionsToClaims(
            user.Roles.SelectMany(x => x.Permissions).Distinct());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                ..userPermissionClaims
            ]),
            Expires = utcNow.AddMinutes(optionsValue.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = optionsValue.Issuer,
            Audience = optionsValue.Audience
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
    }
}
