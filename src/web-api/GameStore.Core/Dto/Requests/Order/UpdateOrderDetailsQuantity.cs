namespace GameStore.Core.Dto.Requests.Order
{
    public record UpdateOrderDetailsQuantity
    {
        public int Count { get; init; }
    }
}