using GameStore.Auth.Functions.Http;
using GameStore.Auth.Functions.Models;
using GameStore.Auth.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace GameStore.Auth.Functions.Functions;

/// <summary>
/// api/users: GET list, POST create (201), PUT update with ?originalEmail= (204), DELETE with a JSON string email body (204).
/// </summary>
public sealed class UsersFunctions(UserStore users)
{
    private const string UsersRoute = "users";

    [Function("GetUsers")]
    public async Task<IActionResult> GetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = UsersRoute)] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var all = await users.GetAllAsync(cancellationToken);
        return new OkObjectResult(all.Select(UserResponse.From).ToList());
    }

    [Function("CreateUser")]
    public async Task<IActionResult> CreateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = UsersRoute)] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var body = await RequestBody.ReadAsync<UserChangeRequest>(request, cancellationToken);
        if (body.Error is not null)
        {
            return body.Error;
        }

        return await ExecuteAsync(
            () => users.CreateAsync(body.Model!.ToUser(), cancellationToken),
            StatusCodes.Status201Created);
    }

    [Function("UpdateUser")]
    public async Task<IActionResult> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = UsersRoute)] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var body = await RequestBody.ReadAsync<UserChangeRequest>(request, cancellationToken);
        if (body.Error is not null)
        {
            return body.Error;
        }

        string? originalEmail = request.Query["originalEmail"];
        return await ExecuteAsync(
            () => users.UpdateAsync(body.Model!.ToUser(), originalEmail, cancellationToken),
            StatusCodes.Status204NoContent);
    }

    [Function("DeleteUser")]
    public async Task<IActionResult> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = UsersRoute)] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var body = await RequestBody.ReadAsync<string>(request, cancellationToken);
        if (body.Error is not null)
        {
            return body.Error;
        }

        return await ExecuteAsync(
            () => users.DeleteAsync(body.Model!, cancellationToken),
            StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// The original service let these errors bubble up as a 500; the message is now included as a ProblemDetails title.
    /// </summary>
    private static async Task<IActionResult> ExecuteAsync(Func<Task> action, int successStatusCode)
    {
        try
        {
            await action();
            return new StatusCodeResult(successStatusCode);
        }
        catch (UserStoreException ex)
        {
            return new ObjectResult(new ProblemDetails
            {
                Title = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
