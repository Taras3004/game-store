using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;

namespace GameStore.DAL.Filtering.EntityFilters
{
    internal class GameFilter(IEnumerable<IFilterStep<Game, GameFilterDetails>> filterSteps)
        : IEntityFilter<Game, GameFilterDetails>
    {
        public async Task<Expression<Func<Game, bool>>> CreateFilter(GameFilterDetails details)
        {
            Expression<Func<Game, bool>> predicate = PredicateBuilder.True<Game>();

            foreach (var step in filterSteps)
            {
                predicate = await step.Apply(predicate, details);
            }

            return predicate;
        }
    }
}