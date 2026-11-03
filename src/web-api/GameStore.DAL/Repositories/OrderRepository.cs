using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Filtering.Contracts;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Data;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Sql.Repositories
{
    internal class OrderRepository(
        GameStoreDbContext context,
        IEntityFilter<Order, OrderFilterDetails> orderFilter) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            return await context.Orders
                .ApplyTracking(tracking)
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id, token);
        }

        public async Task<Order?> GetCartByCustomerIdAsync(Guid customerId, CancellationToken token = default)
        {
            return await context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.Open, token);
        }

        public async Task<IEnumerable<Order>> GetHistoryAsync(OrderFilterDetails details, CancellationToken token = default)
        {
            var predicate = await orderFilter.CreateFilter(details);

            return await context.Orders
                .AsNoTracking()
                .Include(o => o.OrderProducts)
                .Where(o => o.Status != OrderStatus.Open)
                .Where(predicate)
                .ToListAsync(token);
        }

        public async Task AddAsync(Order order, CancellationToken token = default)
        {
            await context.Orders.AddAsync(order, token);
        }

        public Task UpdateAsync(Order order, CancellationToken token = default)
        {
            if (context.Entry(order).State == EntityState.Detached)
            {
                context.Orders.Update(order);
            }

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            var order = await context.Orders.FindAsync([id], cancellationToken: token);
            if (order != null)
            {
                context.Orders.Remove(order);
            }
        }

        public async Task<int> BatchProductKeyUpdateAsync(string oldKey, string newKey, CancellationToken token = default)
        {
            var query = context.OrderProducts.Where(x => x.ProductKey == oldKey);
            return await query.ExecuteUpdateAsync(x => x.SetProperty(p => p.ProductKey, newKey), token);
        }

        public Task DeleteOrderProductAsync(Guid orderProductId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}