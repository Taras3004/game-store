using System.Linq.Expressions;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities.ConfigurationEntities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL
{
    internal static class EfSpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> inputQueryable,
            Specification<TEntity> specification)
            where TEntity : BaseEntity
        {
            IQueryable<TEntity> queryable = inputQueryable;

            queryable = ApplyCriterias(queryable, specification.CriteriaList);

            queryable = queryable.AsTracking(GetQueryTrackingBehavior(specification.TrackingBehaviour));

            if (specification.IsSplitQuery)
            {
                queryable = queryable.AsSplitQuery();
            }

            queryable = specification.IncludeStrings.Aggregate(queryable,
                (current, includeExpression) =>
                    current.Include(includeExpression));

            if (specification.IsQueryFiltersIgnored)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            return queryable;
        }

        internal static QueryTrackingBehavior GetQueryTrackingBehavior(TrackingBehaviour trackingBehaviour) =>
            trackingBehaviour switch
            {
                TrackingBehaviour.Tracking => QueryTrackingBehavior.TrackAll,
                TrackingBehaviour.NoTracking => QueryTrackingBehavior.NoTracking,
                TrackingBehaviour.NoTrackingWithIdentityResolution => QueryTrackingBehavior.NoTrackingWithIdentityResolution,
                _ => throw new InvalidOperationException()
            };

        public static IQueryable<TEntity> ApplyCriterias<TEntity>(
            IQueryable<TEntity> query,
            IEnumerable<Expression<Func<TEntity, bool>>> criteriaList)
            where TEntity : BaseEntity
        {
            foreach (var criteria in criteriaList)
            {
                query = query.Where(criteria);
            }

            return query;
        }
    }
}
