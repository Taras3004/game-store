using GameStore.Core.Dto.Requests.User;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Enums;
using GameStore.Web.Infrastructure.Authorization.Permissions;
using GameStore.Web.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace GameStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IUserService userService,
        IRoleService roleService,
        ILoginService loginService,
        IEntityAccessCheckerFactory entityAccessCheckerFactory) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken token)
        {
            var result = await loginService.Login(request, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem);
        }

        [HttpPost("access")]
        public async Task<IActionResult> Access([FromBody] UserAccessRequest request, CancellationToken token)
        {
            var accessChecker = entityAccessCheckerFactory.GetAccessChecker(request.TargetPage);
            if (accessChecker is null)
                return BadRequest();

            var hasAccess = await accessChecker.HasAccess(request.TargetId, token);

            if (hasAccess)
                return Ok();

            return Forbid();
        }

        [NeedsPermission(Permission.AddUser)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken token)
        {
            var result = await userService.CreateUser(request, token);

            return result.Match(
                onSuccess: user => CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user),
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewUser)]
        [HttpGet("{id}")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken token)
        {
            var result = await userService.GetUserById(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewUser)]
        [HttpGet]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAllUsers(CancellationToken token)
        {
            var users = await userService.GetAllUsers(token);
            return Ok(users);
        }

        [NeedsPermission(Permission.UpdateUser)]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var result = await userService.UpdateUser(request, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeleteUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken token)
        {
            var result = await userService.DeleteUser(id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewRole)]
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetRolesByUserId(Guid id, CancellationToken token)
        {
            var result = await roleService.GetRolesByUserId(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }
    }
}
