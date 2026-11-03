using GameStore.Core.Dto.Requests.Publisher;
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
    public class PublishersController(
        IPublisherService publisherService,
        IGameService gameService) : ControllerBase
    {
        [NeedsPermission(Permission.AddPublisher)]
        [HttpPost]
        public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherRequest request, CancellationToken token)
        {
            var result = await publisherService.CreatePublisher(request, token);

            return result.Match(
                onSuccess: publisher => CreatedAtAction(nameof(GetPublisherById), new { id = publisher.Id }, publisher),
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("find/{id}")]
        public async Task<IActionResult> GetPublisherById(Guid id, CancellationToken token)
        {
            var result = await publisherService.GetPublisher(id, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{companyName}")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetPublisherByCompanyName(string companyName, CancellationToken token)
        {
            var result = await publisherService.GetPublisherByCompanyName(companyName, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAllPublishers(CancellationToken token)
        {
            var publishers = await publisherService.GetAllPublishers(token);

            return Ok(publishers);
        }

        [NeedsPermission(Permission.UpdatePublisher)]
        [HttpPut]
        public async Task<IActionResult> UpdatePublisher([FromBody] UpdatePublisherRequest request, CancellationToken token)
        {
            var result = await publisherService.UpdatePublisher(request, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeletePublisher)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(Guid id, CancellationToken token)
        {
            var result = await publisherService.DeletePublisher(id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("{companyName}/games")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetPublisherGames(string companyName, CancellationToken token)
        {
            var games = await gameService.GetGamesByPublisherCompanyName(companyName, token);

            return Ok(games);
        }
    }
}
