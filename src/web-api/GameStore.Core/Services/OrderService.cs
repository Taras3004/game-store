using AutoMapper;
using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Dto.Requests.Order;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Services
{
    internal class OrderService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider) : IOrderService
    {
        public async Task<Result> AddProductToCartAsync(Guid customerId, string productKey, CancellationToken token = default)
        {
            var cart = await unitOfWork.Orders.GetCartByCustomerIdAsync(customerId, token);

            if (cart == null)
            {
                cart = new Order(customerId, dateTimeProvider.UtcNow, OrderStatus.Open);
                await unitOfWork.Orders.AddAsync(cart, token);
            }

            var game = await unitOfWork.Games.GetByKeyAsync(productKey, token: token);
            if (game == null)
            {
                return Result.Failure(GameErrors.NotFound);
            }

            var existingItem = cart.OrderProducts.FirstOrDefault(x => x.ProductKey == productKey);
            int currentQuantityInCart = existingItem?.Quantity ?? 0;

            if (game.UnitInStock <= currentQuantityInCart)
            {
                return Result.Failure(GameErrors.OutOfStock);
            }

            if (existingItem != null)
            {
                existingItem.IncrementQuantity();
            }
            else
            {
                var orderProduct = new OrderProduct(cart, productKey, game.GamePrice.Price, game.GamePrice.Discount);
                cart.AddToCart(orderProduct);
            }

            await unitOfWork.SaveChangesAsync(token);
            return Result.Success();
        }

        public async Task<Result> RemoveProductFromCustomerCartAsync(Guid customerId, string productKey, CancellationToken token = default)
        {
            var cart = await unitOfWork.Orders.GetCartByCustomerIdAsync(customerId, token);
            if (cart == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            var existingItem = cart.OrderProducts.FirstOrDefault(x => x.ProductKey == productKey);
            if (existingItem != null)
            {
                cart.RemoveFromCart(productKey);
                await unitOfWork.SaveChangesAsync(token);
            }

            return Result.Success();
        }

        public async Task<Result<OrderResponse>> GetCartByCustomerId(Guid customerId, CancellationToken token = default)
        {
            var cart = await unitOfWork.Orders.GetCartByCustomerIdAsync(customerId, token);

            if (cart == null)
            {
                return Result.Failure<OrderResponse>(OrderErrors.NotFound);
            }

            return mapper.Map<OrderResponse>(cart);
        }

        public async Task<Result<OrderResponse>> GetOrderById(Guid orderId, CancellationToken token = default)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(orderId, token: token);

            if (order == null)
            {
                return Result.Failure<OrderResponse>(OrderErrors.NotFound);
            }

            return mapper.Map<OrderResponse>(order);
        }

        public async Task<Result<IEnumerable<OrderProductResponse>>> GetOrderDetails(Guid orderId, CancellationToken token = default)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(orderId, token: token);

            if (order == null)
            {
                return Result.Failure<IEnumerable<OrderProductResponse>>(OrderErrors.NotFound);
            }

            return Result.Success(mapper.Map<IEnumerable<OrderProductResponse>>(order.OrderProducts));
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersHistory(OrderFilterDetails details, CancellationToken token = default)
        {
            var history = await unitOfWork.Orders.GetHistoryAsync(details, token);
            return mapper.Map<IEnumerable<OrderResponse>>(history);
        }

        public async Task<Result> SetOrderProductQuantity(UpdateOrderDetailsQuantity request, Guid id, CancellationToken token = default)
        {
            var orderProduct = await unitOfWork.OrderProducts.GetByIdAsync(id, token);
            if (orderProduct == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            orderProduct.SetQuantity(request.Count);
            
            await unitOfWork.SaveChangesAsync(token);

            return Result.Success();
        }

        public async Task<Result> AddProductToOrderAsync(Guid id, string productKey, CancellationToken token = default)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(id, TrackingBehaviour.Tracking, token);
            if (order == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            var game = await unitOfWork.Games.GetByKeyAsync(productKey, token: token);
            if (game == null)
            {
                return Result.Failure(GameErrors.NotFound);
            }

            var existingItem = order.OrderProducts.FirstOrDefault(x => x.ProductKey == productKey);
            int currentQuantityInOrder = existingItem?.Quantity ?? 0;

            if (game.UnitInStock <= currentQuantityInOrder)
            {
                return Result.Failure(GameErrors.OutOfStock);
            }

            if (existingItem != null)
            {
                existingItem.IncrementQuantity();
            }
            else
            {
                var orderProduct = new OrderProduct(order, productKey, game.GamePrice.Price, game.GamePrice.Discount);
                order.AddToOrder(orderProduct);
            }

            await unitOfWork.SaveChangesAsync(token);
            return Result.Success();
        }

        public async Task<Result> DeleteOrderProduct(Guid id, CancellationToken token)
        {
            var orderProduct = await unitOfWork.OrderProducts.GetByIdAsync(id, token);
            if(orderProduct is null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            await unitOfWork.OrderProducts.Delete(orderProduct, token);
            return Result.Success();
        }

        public async Task<Result> ShipOrder(Guid id, CancellationToken token)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(id, TrackingBehaviour.Tracking, token);
            if (order == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            order.Ship();

            await unitOfWork.SaveChangesAsync(token);
            return Result.Success();
        }
    }
}