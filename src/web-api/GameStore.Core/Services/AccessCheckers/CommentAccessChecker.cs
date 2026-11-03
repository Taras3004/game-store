using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class CommentAccessChecker(ICommentService commentService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await commentService.GetCommentById(targetId, token);
            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
