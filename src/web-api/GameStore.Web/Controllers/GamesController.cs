using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Dto.Requests.Game;
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
    public class GamesController(
        IGameService gameService,
        IOrderService orderService,
        IPublisherService publisherService,
        IGenreService genreService,
        IPlatformService platformService,
        IUserProvider userProvider) : ControllerBase
    {
        [NeedsPermission(Permission.AddGame)]
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request, CancellationToken token)
        {
            var result = await gameService.CreateGame(request, token);

            return result.Match(
                onSuccess: game => CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game),
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("find/{id}")]
        public async Task<IActionResult> GetGameById(Guid id, CancellationToken token)
        {
            var result = await gameService.GetGameById(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetGameByKey(string key, CancellationToken token)
        {
            var result = await gameService.GetGameByKey(key, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames([FromQuery] GameFilterDetails details, CancellationToken token)
        {
            var result = await gameService.GetAllGames(details, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.UpdateGame)]
        [HttpPut]
        public async Task<IActionResult> UpdateGame([FromBody] UpdateGameRequest request, CancellationToken token)
        {
            var result = await gameService.UpdateGame(request, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeleteGame)]
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteGame(string key, CancellationToken token)
        {
            var result = await gameService.DeleteGame(key, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{key}/genres")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetGenresByGameKey(string key, CancellationToken token)
        {
            var result = await genreService.GetGenresByGameKey(key, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{key}/platforms")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetPlatformsByGameKey(string key, CancellationToken token)
        {
            var platforms = await platformService.GetPlatformsByGameKey(key, token);

            return Ok(platforms);
        }

        [HttpGet("{key}/file")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> DownloadGameFile(string key, CancellationToken token)
        {
            var result = await gameService.DownloadGameFile(key, token);

            return result.Match(
                onSuccess: fileResponse => File(fileResponse.FileContent, fileResponse.ContentType, fileResponse.FileName),
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.CreateOrder)]
        [HttpPost("{key}/buy")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> AddGameToCartAsync(string key, CancellationToken token = default)
        {
            var userId = userProvider.GetUserId();
            if (userId is null)
                return Unauthorized();
            var result = await orderService.AddProductToCartAsync(userId.Value, key, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{key}/publisher")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetPublisherByGameKey(string key, CancellationToken token)
        {
            var result = await publisherService.GetPublisherByGameKey(key, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("pagination-options")]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
        public IActionResult GetPaginationOptions()
        {
            return Ok(gameService.GetPaginationOptions());
        }

        [HttpGet("sorting-options")]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
        public IActionResult GetSortingOptions()
        {
            return Ok(gameService.GetSortingOptions());
        }

        [HttpGet("publish-date-options")]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
        public IActionResult GetDateFilteringOptions()
        {
            return Ok(gameService.GetDateFilteringOptions());
        }
    }
}
