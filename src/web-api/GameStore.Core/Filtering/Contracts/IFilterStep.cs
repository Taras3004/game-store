using System.Linq.Expressions;

namespace GameStore.Core.Filtering.Contracts
{
    public interface IFilterStep<TEntity, in TDetails>
        where TEntity : class
    {
        ValueTask<Expression<Func<TEntity, bool>>> Apply(Expression<Func<TEntity, bool>> currentPredicate, TDetails details);
    }
}
