using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Models.ValueObjects
{
    public record GamePrice
    {
        public double Price { get; private init; }
        public int Discount { get; private init; }

        private GamePrice(double price, int discount)
        {
            Price = price;
            Discount = discount;
        }

        public static Result<GamePrice> Create(double price, int discount)
        {
            if (price < 0)
                return Result.Failure<GamePrice>(GameErrors.PriceBelowZero);

            if (discount is < 0 or > 100)
                return Result.Failure<GamePrice>(GameErrors.DiscountOutOfRange);

            GamePrice gamePrice = new(price, discount);

            return gamePrice;
        }

        public double CalculateFinalPrice()
        {
            return Price - (Price * Discount / 100.0);
        }
    }
}
