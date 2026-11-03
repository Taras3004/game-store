using GameStore.Core.Dto.Requests.Ban;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Enums;
using GameStore.Web.Infrastructure.Authorization.Permissions;
using GameStore.Web.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class BansController(IBanService banService) : ControllerBase
    {
        [NeedsPermission(Permission.BanUserFromCommenting)]
        [HttpPost("comments/ban")]
        public async Task<IActionResult> BanUser(BanUserRequest request, CancellationToken token = default)
        {
            var result = await banService.BanUser(request, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.BanUserFromCommenting)]
        [HttpGet("comments/ban/durations")]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
        public IActionResult GetBanDurations()
        {
            return Ok(banService.GetBanDurations());
        }
    }
}