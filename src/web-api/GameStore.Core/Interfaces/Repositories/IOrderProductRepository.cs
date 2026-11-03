using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface IOrderProductRepository : IRepository<OrderProduct>
    {
        Task<OrderProduct?> GetByIdAsync(Guid id, CancellationToken token);
        Task<int> SetQuantityAtomically(Specification<OrderProduct> specification, int quantity, CancellationToken token = default);
        Task DeleteOrderProductAsync(OrderProduct orderProduct, CancellationToken token = default);
    }
}