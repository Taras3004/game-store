using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.EntityFilters
{
    internal class OrderFilter(IEnumerable<IFilterStep<Order, OrderFilterDetails>> filterSteps)
        : IEntityFilter<Order, OrderFilterDetails>
    {
        public async Task<Expression<Func<Order, bool>>> CreateFilter(OrderFilterDetails details)
        {
            Expression<Func<Order, bool>> predicate = PredicateBuilder.True<Order>();

            foreach (var step in filterSteps)
            {
                predicate = await step.Apply(predicate, details);
            }

            return predicate;
        }
    }
}
