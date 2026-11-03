using GameStore.Models.Results;

namespace GameStore.Models.Errors
{
    public static class OrderErrors
    {
        public static readonly Error NotFound = new(
            "Order.NotFound",
            "Order with the specified orderId was not found",
            ErrorType.NotFound
        );

        public static readonly Error AlreadyProcessedOrder = new(
            "Order.AlreadyProcessedOrder",
            "Can not change already processed order",
            ErrorType.Conflict
        );

        public static readonly Error ProductIsNotInTheCart = new(
            "Order.ProductIsNotInTheCart",
            "No product with this orderId in the cart",
            ErrorType.NotFound
        );

        public static readonly Error ProductIsOutOfStock = new(
            "Order.ProductIsOutOfStock",
            "Product in order is out of stock",
            ErrorType.Conflict
        );

        public static readonly Error OnlyPaidCanBeShipped = new(
            "Order.OnlyPaidCanBeShipped",
            "Only paid orders can be shipped",
            ErrorType.Conflict
        );
    }
}
