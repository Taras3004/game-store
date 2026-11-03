using GameStore.Models.Constants;
using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;
using GameStore.Models.Enums;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Models.Entities
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public DateTime? Date { get; private set; }

        public Guid CustomerId { get; private set; }
        public User User { get; private set; }

        public OrderStatus Status { get; private set; }

        public IReadOnlyCollection<OrderProduct> OrderProducts => orderProducts;
        private readonly List<OrderProduct> orderProducts = new();

        public string? RequiredDate { get; init; }
        public string? ShippedDate { get; init; }
        public string? Freight { get; init; }
        public string? ShipName { get; init; }
        public string? ShipAddress { get; init; }
        public string? ShipCity { get; init; }
        public string? ShipRegion { get; init; }
        public int? ShipPostalCode { get; init; }
        public string? ShipCountry { get; init; }

        public string? LegacyId { get; init; }

        private Order()
        {
            User = null!;
        }

        public Order(Guid customerId, DateTime date, OrderStatus status, Guid id = default)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Date = date;
            Status = status;
            CustomerId = customerId;
            User = null!;
        }

        public double CalculateTransactionAmount() => orderProducts
            .Sum(x => (x.Price - x.Price * x.Discount) * x.Quantity);

        public Result AddToCart(OrderProduct orderProduct)
        {
            if (Status != OrderStatus.Open)
            {
                return Result.Failure(OrderErrors.AlreadyProcessedOrder);
            }

            return AddToOrder(orderProduct);
        }

        public Result AddToOrder(OrderProduct orderProduct)
        {
            var existingOrderProduct = orderProducts.FirstOrDefault(x => x.ProductKey == orderProduct.ProductKey);

            if (existingOrderProduct == null)
            {
                existingOrderProduct = new OrderProduct(this, orderProduct.ProductKey, orderProduct.Price, orderProduct.Discount);
                orderProducts.Add(existingOrderProduct);
            }
            else
            {
                existingOrderProduct.SetQuantity(existingOrderProduct.Quantity + orderProduct.Quantity);
            }

            return Result.Success();
        }

        public Result RemoveFromCart(string productKey)
        {
            if (Status != OrderStatus.Open)
            {
                return Result.Failure(OrderErrors.AlreadyProcessedOrder);
            }

            return RemoveFromOrder(productKey);
        }

        public Result RemoveFromOrder(string productKey)
        {
            var orderProduct = orderProducts.FirstOrDefault(x => x.ProductKey == productKey);

            if (orderProduct == null)
            {
                return Result.Failure(OrderErrors.ProductIsNotInTheCart);
            }

            orderProducts.Remove(orderProduct);

            return Result.Success();
        }

        public void Paid(string paymentMethod)
        {
            Status = paymentMethod switch
            {
                PaymentMethods.BankMethodName => OrderStatus.Checkout,
                PaymentMethods.IBoxMethodName or PaymentMethods.VisaMethodName => OrderStatus.Paid,
                _ => throw new InvalidOperationException()
            };
        }

        public void Cancelled()
        {
            Status = OrderStatus.Cancelled;
        }

        public List<IDomainEvent> DomainEvents { get; } = [];
        public void Raise(IDomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }

        public Result Ship()
        {
            if (Status != OrderStatus.Paid)
                return Result.Failure(OrderErrors.OnlyPaidCanBeShipped);
            
            Status = OrderStatus.Shipped;
            return Result.Success();
        }
    }
}