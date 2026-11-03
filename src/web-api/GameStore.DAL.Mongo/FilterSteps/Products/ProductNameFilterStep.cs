using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.FilterSteps.Products
{
    internal class ProductNameFilterStep : IFilterStep<Product, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Product, bool>>> Apply(Expression<Func<Product, bool>> currentPredicate, GameFilterDetails details)
        {
            if (!string.IsNullOrWhiteSpace(details.Name))
            {
                string name = details.Name.ToLower();
                currentPredicate = currentPredicate.And(x => x.ProductName.ToLower().Contains(name));
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
