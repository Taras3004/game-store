using System.Globalization;
using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Filtering.Contracts;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Mappings;
using GameStore.DAL.Mongo.Normalization;
using GameStore.Models.Enums;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DAL.Mongo.Repositories
{
    using MongoOrderEntity = Entities.Order;
    using Order = Models.Entities.Order;

    internal class MongoOrderRepository(
        INorthwindQuerySource<MongoOrderEntity> orderSource,
        MongoKeyResolver<MongoOrderEntity> keyResolver,
        IEntityFilter<MongoOrderEntity, OrderFilterDetails> orderFilter) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var legacyId = await keyResolver.GetLegacyIdAsync(id);

            if (legacyId is null || !int.TryParse(legacyId, out int orderId))
            {
                return null;
            }

            var mongoOrder = await orderSource
                .Query()
                .FirstOrDefaultAsync(x => x.OrderId == orderId, token);

            return mongoOrder is null ? null : MapToDomain(mongoOrder);
        }

        public Task<Order?> GetCartByCustomerIdAsync(Guid customerId, CancellationToken token = default)
        {
            return Task.FromResult<Order?>(null);
        }

        public async Task<IEnumerable<Order>> GetHistoryAsync(OrderFilterDetails details, CancellationToken token = default)
        {
            var predicate = await orderFilter.CreateFilter(details);

            var mongoOrders = await orderSource
                .Query()
                .Where(predicate)
                .ToListAsync(token);

            return mongoOrders
                .Select(MapToDomain)
                .ToList();
        }

        public Task AddAsync(Order order, CancellationToken token = default)
            => throw new NotSupportedException("Cannot create new orders in the legacy Northwind database.");

        public Task UpdateAsync(Order order, CancellationToken token = default)
            => throw new NotSupportedException("Cannot update orders in the legacy Northwind database.");

        public Task DeleteAsync(Guid id, CancellationToken token = default)
            => throw new NotSupportedException("Cannot delete orders from the legacy Northwind database.");

        public Task<int> BatchProductKeyUpdateAsync(string oldKey, string newKey, CancellationToken token = default)
            => throw new NotSupportedException("Key updates must be performed in the primary SQL database.");

        public Task DeleteOrderProductAsync(Guid orderProductId, CancellationToken token = default)
            => throw new NotSupportedException("Cannot delete in the legacy Northwind database.");

        public Task BatchProductKeyUpdate(string oldKey, string newKey, CancellationToken cancellationToken)
            => throw new NotSupportedException("Cannot change the legacy Northwind database.");

        private static Order MapToDomain(MongoOrderEntity mongoOrder)
        {
            var orderConfig = MongoEntityConfigMap.GetEntityConfig<MongoOrderEntity>();
            var customerConfig = MongoEntityConfigMap.GetEntityConfig<Customer>();

            string orderSurrogateKey = orderConfig.GetSurrogateKey(mongoOrder);
            Guid orderId = $"{orderConfig.Prefix}{orderSurrogateKey}".ToDeterministicGuid();

            Guid customerId = string.IsNullOrWhiteSpace(mongoOrder.CustomerId)
                ? Guid.Empty
                : $"{customerConfig.Prefix}{mongoOrder.CustomerId}".ToDeterministicGuid();

            // Northwind orders are read-only history: they are either shipped or already paid
            var status = mongoOrder.ShippedDate is null ? OrderStatus.Paid : OrderStatus.Shipped;

            return new Order(customerId, mongoOrder.OrderDate ?? DateTime.MinValue, status, orderId)
            {
                LegacyId = orderSurrogateKey,
                RequiredDate = mongoOrder.RequiredDate?.ToString("O", CultureInfo.InvariantCulture),
                ShippedDate = mongoOrder.ShippedDate?.ToString("O", CultureInfo.InvariantCulture),
                Freight = mongoOrder.Freight?.ToString(CultureInfo.InvariantCulture),
                ShipName = mongoOrder.ShipName,
                ShipAddress = mongoOrder.ShipAddress,
                ShipCity = mongoOrder.ShipCity,
                ShipRegion = mongoOrder.ShipRegion,
                ShipPostalCode = int.TryParse(mongoOrder.ShipPostalCode, out int postalCode) ? postalCode : null,
                ShipCountry = mongoOrder.ShipCountry,
            };
        }
    }
}
