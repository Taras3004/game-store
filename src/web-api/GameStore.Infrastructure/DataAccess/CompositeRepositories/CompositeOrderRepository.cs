using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DataAccess.CompositeRepositories
{
    internal class CompositeOrderRepository(
        [FromKeyedServices(DataSourceKeys.PrimaryDataSource)] IOrderRepository sqlRepo,
        [FromKeyedServices(DataSourceKeys.LegacyDataSource)] IOrderRepository mongoRepo) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var order = await sqlRepo.GetByIdAsync(id, tracking, token);
            if (order is not null)
            {
                return order;
            }

            return tracking == TrackingBehaviour.Tracking
                ? null
                : await mongoRepo.GetByIdAsync(id, token: token);
        }

        public async Task<Order?> GetCartByCustomerIdAsync(Guid customerId, CancellationToken token = default)
        {
            return await sqlRepo.GetCartByCustomerIdAsync(customerId, token);
        }

        public async Task<IEnumerable<Order>> GetHistoryAsync(OrderFilterDetails details, CancellationToken token = default)
        {
            var sqlOrders = await sqlRepo.GetHistoryAsync(details, token);
            var mongoOrders = await mongoRepo.GetHistoryAsync(details, token);

            return sqlOrders.Concat(mongoOrders);
        }

        public async Task AddAsync(Order order, CancellationToken token = default)
        {
            await sqlRepo.AddAsync(order, token);
        }

        public async Task UpdateAsync(Order order, CancellationToken token = default)
        {
            await sqlRepo.UpdateAsync(order, token);
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            await sqlRepo.DeleteAsync(id, token);
        }

        public async Task<int> BatchProductKeyUpdateAsync(string oldKey, string newKey, CancellationToken token = default)
        {
            return await sqlRepo.BatchProductKeyUpdateAsync(oldKey, newKey, token);
        }
    }
}
