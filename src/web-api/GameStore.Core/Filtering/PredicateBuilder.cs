using System.Linq.Expressions;

namespace GameStore.Core.Filtering
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = expr1.Parameters[0];

            var visitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
            var body2 = visitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, body2), parameter);
        }
    }
}