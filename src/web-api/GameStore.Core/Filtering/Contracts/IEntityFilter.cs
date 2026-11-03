using System.Linq.Expressions;

namespace GameStore.Core.Filtering.Contracts
{
    public interface IEntityFilter<TEntity, in TDetails>
    {
        Task<Expression<Func<TEntity, bool>>> CreateFilter(TDetails details);
    }
}
