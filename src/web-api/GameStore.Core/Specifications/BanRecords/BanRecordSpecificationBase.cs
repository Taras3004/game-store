using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Specifications.BanRecords
{
    internal abstract class BanRecordSpecificationBase(Expression<Func<BanRecord, bool>>? criteria) : Specification<BanRecord>(criteria);
}