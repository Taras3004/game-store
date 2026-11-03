using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Data;
using GameStore.DAL.Repositories.Base;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    internal class OrderProductRepository(GameStoreDbContext context) : SqlRepository<OrderProduct>(context), IOrderProductRepository
    {
        private readonly GameStoreDbContext context = context;

        public Task DeleteOrderProductAsync(OrderProduct orderProduct, CancellationToken token = default)
        {
            context.Remove(orderProduct);
            return Task.CompletedTask;
        }

        public async Task<OrderProduct?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await context.OrderProducts.FindAsync(id, token);
        }

        public async Task<int> SetQuantityAtomically(Specification<OrderProduct> specification, int quantity, CancellationToken token = default)
        {
            var query = EfSpecificationEvaluator.GetQuery(context.OrderProducts, specification);

            return await query.ExecuteUpdateAsync(x => x
                .SetProperty(p => p.Quantity, quantity), token);
        }
    }
}