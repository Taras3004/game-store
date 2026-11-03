using GameStore.Core.Dto.Details.Comment;

namespace GameStore.Core.Dto.Requests.Comment
{
    public record PostCommentRequest
    {
        public required CommentDetailsBase Comment { get; init; }
        public string? Action { get; init; }
        public Guid? ParentId { get; init; }
    }
}
