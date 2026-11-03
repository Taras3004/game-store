using GameStore.Core.Dto.Requests.Comment;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Enums;
using GameStore.Web.Infrastructure.Authorization.Permissions;
using GameStore.Web.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentsController(ICommentService commentService) : ControllerBase
    {
        [NeedsPermission(Permission.AddComment)]
        [HttpPost("games/{key}/comments")]
        public async Task<IActionResult> PostComment(PostCommentRequest request, [FromRoute] string key, CancellationToken token = default)
        {
            var result = await commentService.PostComment(request, key, token);

            return result.Match(
                onSuccess: Created,
                onFailure: ApiResults.Problem
            );
        }

        [HttpGet("games/{key}/comments")]
        public async Task<IActionResult> GetComments([FromRoute] string key, CancellationToken token = default)
        {
            var result = await commentService.GetComments(key, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeleteOwnComment)]
        [HttpDelete("games/{key}/comments/{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] string key, [FromRoute] Guid id, CancellationToken token = default)
        {
            var result = await commentService.DeleteComment(key, id, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }
    }
}