using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.FilterSteps.Products
{
    internal class ProductPublishersFilterStep(MongoKeyResolver<Supplier> supplierKeyResolver)
        : IFilterStep<Product, GameFilterDetails>
    {
        public async ValueTask<Expression<Func<Product, bool>>> Apply(Expression<Func<Product, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.Publishers != null && details.Publishers.Any())
            {
                var supplierIds = new List<int>();

                foreach (Guid publisherId in details.Publishers)
                {
                    var legacyId = await supplierKeyResolver.GetLegacyIdAsync(publisherId);
                    if (legacyId is not null && int.TryParse(legacyId, out int supplierId))
                    {
                        supplierIds.Add(supplierId);
                    }
                }

                currentPredicate = currentPredicate.And(x => supplierIds.Contains(x.SupplierId));
            }

            return currentPredicate;
        }
    }
}
