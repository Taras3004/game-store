using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default);

        Task<Order?> GetCartByCustomerIdAsync(Guid customerId, CancellationToken token = default);

        Task<IEnumerable<Order>> GetHistoryAsync(OrderFilterDetails details, CancellationToken token = default);

        Task AddAsync(Order order, CancellationToken token = default);
        Task UpdateAsync(Order order, CancellationToken token = default);
        Task DeleteAsync(Guid id, CancellationToken token = default);

        Task<int> BatchProductKeyUpdateAsync(string oldKey, string newKey, CancellationToken token = default);

    }
}
