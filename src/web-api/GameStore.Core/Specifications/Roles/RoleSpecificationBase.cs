using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Specifications.Roles
{
    internal abstract class RoleSpecificationBase(Expression<Func<Role, bool>>? criteria) : Specification<Role>(criteria);
}
