using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;
using System.Linq.Expressions;

namespace GameStore.DAL.Filtering.FilterSteps.Games
{
    internal class GameGenresFilterStep : IFilterStep<Game, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Game, bool>>> Apply(Expression<Func<Game, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.Genres != null && details.Genres.Any())
            {
                var genreIds = details.Genres.ToList();

                currentPredicate = currentPredicate.And(x =>
                    x.GameGenres.Any(gg => genreIds.Contains(gg.GenreId)));
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
