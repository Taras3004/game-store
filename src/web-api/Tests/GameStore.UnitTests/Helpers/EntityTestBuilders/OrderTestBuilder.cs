using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.ValueObjects;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class OrderTestBuilder
    {
        public static Order CreateValidOrder(
            Guid? id = null,
            Guid? customerId = null,
            OrderStatus status = OrderStatus.Open,
            DateTime? date = null)
        {
            var order = new Order(
                customerId ?? Guid.NewGuid(),
                date ?? DateTime.UtcNow,
                status,
                id ?? Guid.Empty);
            return order;
        }

        public static Order CreateValidCart(
            Guid? customerId = null)
        {
            return CreateValidOrder(
                customerId: customerId,
                status: OrderStatus.Open);
        }

        public static Order CreateValidCartWithProduct(
            Guid? customerId = null,
            Game? game = null)
        {
            game ??= GameTestBuilder.CreateValidGame();
            var order = CreateValidCart(customerId: customerId);
            order.AddToCart(new ProductInfo(game.Key, game.GamePrice.Price, game.GamePrice.Discount, game.UnitInStock));
            return order;
        }
    }
}
