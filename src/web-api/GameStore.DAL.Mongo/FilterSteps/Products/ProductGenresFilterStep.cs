using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.FilterSteps.Products
{
    internal class ProductGenresFilterStep(MongoKeyResolver<Category> categoryKeyResolver)
        : IFilterStep<Product, GameFilterDetails>
    {
        public async ValueTask<Expression<Func<Product, bool>>> Apply(Expression<Func<Product, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.Genres != null && details.Genres.Any())
            {
                var categoryIds = new List<int>();

                foreach (Guid genreId in details.Genres)
                {
                    var legacyId = await categoryKeyResolver.GetLegacyIdAsync(genreId);
                    if (legacyId is not null && int.TryParse(legacyId, out int categoryId))
                    {
                        categoryIds.Add(categoryId);
                    }
                }

                currentPredicate = currentPredicate.And(x => categoryIds.Contains(x.CategoryId));
            }

            return currentPredicate;
        }
    }
}
