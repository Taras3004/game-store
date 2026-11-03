using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.FilterSteps.Products
{
    internal class ProductPriceFilterStep : IFilterStep<Product, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Product, bool>>> Apply(Expression<Func<Product, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.MaxPrice.HasValue)
            {
                double maxPrice = details.MaxPrice.Value;
                currentPredicate = currentPredicate.And(x => x.UnitPrice / 100 <= maxPrice);
            }

            if (details.MinPrice.HasValue)
            {
                double minPrice = details.MinPrice.Value;
                currentPredicate = currentPredicate.And(x => x.UnitPrice / 100 >= minPrice);
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
