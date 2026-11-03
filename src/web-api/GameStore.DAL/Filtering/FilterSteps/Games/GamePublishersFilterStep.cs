using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;
using System.Linq.Expressions;

namespace GameStore.DAL.Filtering.FilterSteps.Games
{
    internal class GamePublishersFilterStep : IFilterStep<Game, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Game, bool>>> Apply(Expression<Func<Game, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.Publishers != null && details.Publishers.Any())
            {
                var publisherIds = details.Publishers.ToList();

                currentPredicate = currentPredicate.And(x => publisherIds.Contains(x.PublisherId));
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
