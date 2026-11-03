using GameStore.Models.Entities.ConfigurationEntities;

namespace GameStore.Models.Entities
{
    public class OrderProduct : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }
        public string ProductKey { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get; private set; }
        public int Discount { get; private set; }

        public OrderProduct(Order order, string productKey, double price, int discount)
        {
            Order = order;
            ProductKey = productKey;
            Quantity = 1;

            OrderId = order.Id;
            Price = price;
            Discount = discount;
        }

        private OrderProduct()
        {
            Order = null!;
            ProductKey = null!;
        }

        public void IncrementQuantity()
        {
            Quantity++;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
