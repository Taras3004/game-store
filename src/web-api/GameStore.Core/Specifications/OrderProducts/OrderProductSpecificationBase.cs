using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Specifications.OrderProducts
{
    internal abstract class OrderProductSpecificationBase(Expression<Func<OrderProduct, bool>>? criteria)
        : Specification<OrderProduct>(criteria)
    {
        public OrderProductSpecificationBase WithOrders()
        {
            AddInclude(x => x.Order);

            return this;
        }
    }
}