using GameStore.Core.Dto.Requests.Comment;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task<Result> PostComment(PostCommentRequest request, string gameKey, CancellationToken token = default);
        Task<Result<IEnumerable<CommentResponse>>> GetComments(string gameKey, CancellationToken token = default);
        Task<Result> DeleteComment(string gameKey, Guid commentId, CancellationToken token = default);
        Task<Result<CommentResponse>> GetCommentById(Guid id, CancellationToken token = default);
    }
}