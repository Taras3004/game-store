using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Specifications.Users
{
    internal abstract class UserSpecificationBase(Expression<Func<User, bool>>? criteria) : Specification<User>(criteria)
    {
        public UserSpecificationBase WithRoles()
        {
            AddInclude(x => x.Roles);

            return this;
        }
    }
}
