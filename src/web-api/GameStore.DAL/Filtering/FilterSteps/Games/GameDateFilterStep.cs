using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;
using System.Linq.Expressions;
using GameStore.Core.Interfaces.Services;

namespace GameStore.DAL.Filtering.FilterSteps.Games
{
    internal class GameDateFilterStep(IDateTimeProvider dateTimeProvider) : IFilterStep<Game, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Game, bool>>> Apply(Expression<Func<Game, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.DatePublishing != null
                && FilteringMappings.DateFilteringMap
                    .TryGetValue(details.DatePublishing, out TimeSpan timeSpan))
            {
                DateTime date = dateTimeProvider.UtcNow - timeSpan;
                currentPredicate = currentPredicate.And(x => x.PublishingDate != null && x.PublishingDate > date);
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
