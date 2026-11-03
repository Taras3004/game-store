using GameStore.Core.Dto.Requests.Genre;
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
    public class GenresController(
        IGenreService genreService,
        IGameService gameService) : ControllerBase
    {
        [NeedsPermission(Permission.AddGenre)]
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest request, CancellationToken token)
        {
            var result = await genreService.CreateGenre(request, token);

            return result.Match(
                onSuccess: genre => CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre),
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{id}")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetGenreById(Guid id, CancellationToken token)
        {
            var result = await genreService.GetGenreById(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAllGenres(CancellationToken token)
        {
            var genres = await genreService.GetAllGenres(token);
            return Ok(genres);
        }

        [NeedsPermission(Permission.UpdateGenre)]
        [HttpPut]
        public async Task<IActionResult> UpdateGenre([FromBody] UpdateGenreRequest request, CancellationToken token)
        {
            var result = await genreService.UpdateGenre(request, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeleteGenre)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(Guid id, CancellationToken token)
        {
            var result = await genreService.DeleteGenre(id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{id}/games")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetGamesByGenreId(Guid id, CancellationToken token = default)
        {
            var games = await gameService.GetGamesByGenreId(id, token);

            return Ok(games);
        }

        [HttpGet("{id}/genres")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetChildGenres(Guid id, CancellationToken token = default)
        {
            var genres = await genreService.GetChildGenres(id, token);

            return Ok(genres);
        }

        [NeedsPermission(Permission.UpdateGenre)]
        [HttpPut("{id}/parent")]
        public async Task<IActionResult> SetParentGenre(Guid id, [FromBody] Guid parentId, CancellationToken token = default)
        {
            var result = await genreService.AddParentGenre(id, parentId, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }
    }
}
