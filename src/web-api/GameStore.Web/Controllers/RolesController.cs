using GameStore.Core.Dto.Requests.Role;
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
    public class RolesController(
        IRoleService roleService) : ControllerBase
    {
        [NeedsPermission(Permission.AddRole)]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken token)
        {
            var result = await roleService.CreateRole(request, token);

            return result.Match(
                onSuccess: role => CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role),
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewRole)]
        [HttpGet("{id}")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetRoleById(Guid id, CancellationToken token)
        {
            var result = await roleService.GetRoleById(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewRole)]
        [HttpGet]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAllRoles(CancellationToken token)
        {
            var roles = await roleService.GetAllRoles(token);
            return Ok(roles);
        }

        [NeedsPermission(Permission.UpdateRole)]
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request, CancellationToken token)
        {
            var result = await roleService.UpdateRole(request, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeleteRole)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id, CancellationToken token)
        {
            var result = await roleService.DeleteRole(id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewRole)]
        [HttpDelete("{id}/permissions")]
        public async Task<IActionResult> GetPermissionsByRoleId(Guid id, CancellationToken token)
        {
            var result = await roleService.GetPermissionsByRoleId(id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewRole)]
        [HttpDelete("permissions")]
        public IActionResult GetAllPermissions()
        {
            return Ok(roleService.GetAllPermissions());
        }
    }
}
