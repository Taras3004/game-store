using System.Linq.Expressions;

namespace GameStore.Core.Filtering
{
    internal class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ReferenceEquals(node, oldParameter) ? newParameter : base.VisitParameter(node);
        }
    }
}