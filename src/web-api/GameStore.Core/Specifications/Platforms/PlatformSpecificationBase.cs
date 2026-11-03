using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Specifications.Platforms
{
    internal abstract class PlatformSpecificationBase(Expression<Func<Platform, bool>>? criteria) : Specification<Platform>(criteria);
}