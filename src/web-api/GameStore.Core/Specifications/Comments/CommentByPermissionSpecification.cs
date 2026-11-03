using GameStore.Core.Dto.Details.Comment;

namespace GameStore.Core.Specifications.Comments
{
    internal class CommentByPermissionSpecification
        : CommentSpecificationBase
    {
        public CommentByPermissionSpecification(CommentPermissionDetails? details = null) : base(null)
        {
            if(details is null)
                return;

            if (details.AccessDeleted)
            {
                IgnoreQueryFilters();
            }
        }
    }
}