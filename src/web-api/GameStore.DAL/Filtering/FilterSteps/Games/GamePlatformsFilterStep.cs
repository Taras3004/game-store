using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;
using System.Linq.Expressions;

namespace GameStore.DAL.Filtering.FilterSteps.Games
{
    internal class GamePlatformsFilterStep : IFilterStep<Game, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Game, bool>>> Apply(Expression<Func<Game, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.Platforms != null)
            {
                var platformIds = details.Platforms.ToList();
                currentPredicate = currentPredicate.And(x => x.GamePlatforms.Any(gp => platformIds.Contains(gp.PlatformId)));
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
