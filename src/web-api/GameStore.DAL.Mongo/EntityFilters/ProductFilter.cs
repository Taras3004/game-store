using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.EntityFilters
{
    internal class ProductFilter(IEnumerable<IFilterStep<Product, GameFilterDetails>> filterSteps)
        : IEntityFilter<Product, GameFilterDetails>
    {
        public async Task<Expression<Func<Product, bool>>> CreateFilter(GameFilterDetails details)
        {
            Expression<Func<Product, bool>> predicate = PredicateBuilder.True<Product>();

            foreach (var step in filterSteps)
            {
                predicate = await step.Apply(predicate, details);
            }

            return predicate;
        }
    }
}
