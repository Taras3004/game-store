using GameStore.Core.Specifications.BaseClass;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL
{
    internal static class QueryableTrackingExtensions
    {
        public static IQueryable<TEntity> ApplyTracking<TEntity>(
            this IQueryable<TEntity> queryable,
            TrackingBehaviour trackingBehaviour)
            where TEntity : class
        {
            return queryable.AsTracking(EfSpecificationEvaluator.GetQueryTrackingBehavior(trackingBehaviour));
        }
    }
}
