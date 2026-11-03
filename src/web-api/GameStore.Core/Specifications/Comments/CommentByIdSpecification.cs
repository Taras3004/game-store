using GameStore.Core.Dto.Details.Comment;

namespace GameStore.Core.Specifications.Comments
{
    internal class CommentByIdSpecification : CommentByPermissionSpecification
    {
        public CommentByIdSpecification(Guid commentId, CommentPermissionDetails? details = null) : base(details)
        {
            AddCriteria(comment => comment.Id == commentId);
        }
    }
}