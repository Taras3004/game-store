using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.FilterSteps.Orders
{
    internal class OrderDateFilterStep : IFilterStep<Order, OrderFilterDetails>
    {
        public ValueTask<Expression<Func<Order, bool>>> Apply(Expression<Func<Order, bool>> currentPredicate, OrderFilterDetails details)
        {
            if (details.StartDate is not null)
            {
                DateTime? startOfDay = details.StartDate.Value.Date;

                currentPredicate = currentPredicate.And(x => x.OrderDate != null && x.OrderDate >= startOfDay);
            }

            if (details.EndDate is not null)
            {
                DateTime? nextDay = details.EndDate.Value.Date.AddDays(1);

                currentPredicate = currentPredicate.And(x => x.OrderDate != null && x.OrderDate < nextDay);
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
