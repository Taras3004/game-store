using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Dto.Requests.Order;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Result> AddProductToCartAsync(Guid customerId, string productKey, CancellationToken token = default);
        Task<Result> RemoveProductFromCustomerCartAsync(Guid customerId, string productKey, CancellationToken token = default);
        Task<Result<OrderResponse>> GetCartByCustomerId(Guid customerId, CancellationToken token = default);
        Task<Result<OrderResponse>> GetOrderById(Guid orderId, CancellationToken token = default);
        Task<Result<IEnumerable<OrderProductResponse>>> GetOrderDetails(Guid orderId, CancellationToken token = default);
        Task<IEnumerable<OrderResponse>> GetOrdersHistory(OrderFilterDetails details, CancellationToken token = default);
        Task<Result> SetOrderProductQuantity(UpdateOrderDetailsQuantity request, Guid id, CancellationToken token = default);
        Task<Result> AddProductToOrderAsync(Guid id, string productKey, CancellationToken token = default);
        Task<Result> DeleteOrderProduct(Guid id, CancellationToken token);
        Task<Result> ShipOrder(Guid id, CancellationToken token);
    }
}
