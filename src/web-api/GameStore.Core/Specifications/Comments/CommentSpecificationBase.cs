using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Specifications.Comments
{
    internal abstract class CommentSpecificationBase(Expression<Func<Comment, bool>>? criteria) : Specification<Comment>(criteria)
    {
        public CommentSpecificationBase WithChildComments()
        {
            AddInclude(x => x.ChildComments);

            return this;
        }

        public CommentSpecificationBase WithParentComment()
        {
            AddInclude(x => x.ParentComment!);

            return this;
        }
    }
}