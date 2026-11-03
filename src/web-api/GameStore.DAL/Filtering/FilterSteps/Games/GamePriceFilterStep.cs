using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering;
using GameStore.Core.Filtering.Contracts;
using GameStore.Models.Entities;
using System.Linq.Expressions;

namespace GameStore.DAL.Filtering.FilterSteps.Games
{
    internal class GamePriceFilterStep : IFilterStep<Game, GameFilterDetails>
    {
        public ValueTask<Expression<Func<Game, bool>>> Apply(Expression<Func<Game, bool>> currentPredicate, GameFilterDetails details)
        {
            if (details.MaxPrice.HasValue)
            {
                currentPredicate = currentPredicate.And(x => x.GamePrice.Price - x.GamePrice.Price * x.GamePrice.Discount / 100 <= details.MaxPrice.Value);
            }

            if (details.MinPrice.HasValue)
            {
                currentPredicate = currentPredicate.And(x => x.GamePrice.Price - x.GamePrice.Price * x.GamePrice.Discount / 100 >= details.MinPrice.Value);
            }

            return ValueTask.FromResult(currentPredicate);
        }
    }
}
