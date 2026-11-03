using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;

namespace GameStore.DAL.Filtering.FilterSteps.Orders
{
    internal class OrderDateFilterStep : IFilterStep<Order, OrderFilterDetails>
    {
        public ValueTask<Expression<Func<Order, bool>>> Apply(Expression<Func<Order, bool>> currentPredicate, OrderFilterDetails details)
        {
            if (details.StartDate is not null)
            {
                var startOfDay = details.StartDate.Value.Date;

                currentPredicate = currentPredicate.And(x => x.Date != null && x.Date >= startOfDay);
            }

            if (details.EndDate is not null)
            {
                var nextDay = details.EndDate.Value.Date.AddDays(1);

                currentPredicate = currentPredicate.And(x => x.Date != null && x.Date < nextDay);
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
