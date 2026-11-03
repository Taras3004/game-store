using System.Linq.Expressions;
using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;

namespace GameStore.DAL.Filtering.FilterSteps.Games
{
    internal class GameNameFilterStep : IFilterStep<Game, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Game, bool>>> Apply(Expression<Func<Game, bool>> currentPredicate, GameFilterDetails details)
        {
            if (!string.IsNullOrWhiteSpace(details.Name))
            {
                currentPredicate = currentPredicate.And(x => x.Name.Contains(details.Name));
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
