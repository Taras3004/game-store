using GameStore.Core.Dto.Requests.Platform;
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
    public class PlatformsController(
        IPlatformService platformService,
        IGameService gameService) : ControllerBase
    {
        [NeedsPermission(Permission.AddPlatform)]
        [HttpPost]
        public async Task<IActionResult> CreatePlatform([FromBody] CreatePlatformRequest request, CancellationToken token)
        {
            var result = await platformService.CreatePlatform(request, token);

            return result.Match(
                onSuccess: platform => CreatedAtAction(nameof(GetPlatform), new { id = platform.Id }, platform),
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{id}")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetPlatform(Guid id, CancellationToken token)
        {
            var result = await platformService.GetPlatform(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlatforms(CancellationToken token)
        {
            var platforms = await platformService.GetAllPlatforms(token);

            return Ok(platforms);
        }

        [NeedsPermission(Permission.UpdatePlatform)]
        [HttpPut]
        public async Task<IActionResult> UpdatePlatform([FromBody] UpdatePlatformRequest request, CancellationToken token)
        {
            var result = await platformService.UpdatePlatform(request, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeletePlatform)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(Guid id, CancellationToken token)
        {
            var result = await platformService.DeletePlatform(id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{id}/games")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetGamesByPlatformId(Guid id, CancellationToken token = default)
        {
            var games = await gameService.GetGamesByPlatformId(id, token);

            return Ok(games);
        }
    }
}
