using System.Linq.Expressions;

namespace GameStore.Core.Specifications.BaseClass
{
    public abstract class Specification<TEntity>
        where TEntity : class
    {
        public IReadOnlyList<Expression<Func<TEntity, bool>>> CriteriaList => criteriaList;
        private readonly List<Expression<Func<TEntity, bool>>> criteriaList = [];

        public List<string> IncludeStrings { get; } = new();

        public TrackingBehaviour TrackingBehaviour { get; private set; } = TrackingBehaviour.NoTracking;

        public bool IsSplitQuery { get; protected set; } = false;

        public bool IsQueryFiltersIgnored { get; protected set; }

        protected Specification(Expression<Func<TEntity, bool>>? criteria)
        {
            AddCriteria(criteria);
        }

        protected Specification() { }

        protected void AddInclude<TNav, TSubNav>(
            Expression<Func<TEntity, IEnumerable<TNav>>> includeExpression,
            Expression<Func<TNav, TSubNav>> innerIncludeExpression)
        {
            var include = GetMemberName(includeExpression);
            var then = GetMemberName(innerIncludeExpression);
            IncludeStrings.Add($"{include}.{then}");
        }

        protected void AddInclude<TNav, TSubNav>(
            Expression<Func<TEntity, TNav>> includeExpression,
            Expression<Func<TNav, TSubNav>> innerIncludeExpression)
        {
            var include = GetMemberName(includeExpression);
            var then = GetMemberName(innerIncludeExpression);
            IncludeStrings.Add($"{include}.{then}");
        }

        protected void AddInclude(
            Expression<Func<TEntity, object>> includeExpression)
        {
            var include = GetMemberName(includeExpression);

            IncludeStrings.Add($"{include}");
        }

        public Specification<TEntity> AsTracking()
        {
            TrackingBehaviour = TrackingBehaviour.Tracking;
            return this;
        }

        public Specification<TEntity> AsNoTrackingWithIdentityResolution()
        {
            TrackingBehaviour = TrackingBehaviour.NoTrackingWithIdentityResolution;
            return this;
        }

        public void AddCriteria(Expression<Func<TEntity, bool>>? criteria)
        {
            if (criteria == null)
                return;
            criteriaList.Add(criteria);
        }

        public void IgnoreQueryFilters()
        {
            IsQueryFiltersIgnored = true;
        }

        private static string GetMemberName<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            throw new ArgumentException("Can't get member's name");
        }
    }
}
