using GameStore.Auth.Functions.Http;
using GameStore.Auth.Functions.Models;
using GameStore.Auth.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace GameStore.Auth.Functions.Functions;

/// <summary>
/// POST api/auth: returns the user (without password) on success, or 401.
/// </summary>
public sealed class AuthFunctions(UserStore users)
{
    [Function("Authenticate")]
    public async Task<IActionResult> Authenticate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var body = await RequestBody.ReadAsync<AuthRequest>(request, cancellationToken);
        if (body.Error is not null)
        {
            return body.Error;
        }

        var user = await users.AuthenticateAsync(body.Model!.Email, body.Model.Password, cancellationToken);
        return user is null
            ? new UnauthorizedResult()
            : new OkObjectResult(UserResponse.From(user));
    }
}
