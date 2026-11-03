using GameStore.Core.Dto.Details.Comment;

namespace GameStore.Core.Specifications.Comments
{
    internal class CommentByGameKeySpecification : CommentByPermissionSpecification
    {
        public CommentByGameKeySpecification(string gameKey, CommentPermissionDetails? permissions = null) : base(permissions)
        {
            AddCriteria(comment => comment.ProductKey == gameKey);
        }
    }
}