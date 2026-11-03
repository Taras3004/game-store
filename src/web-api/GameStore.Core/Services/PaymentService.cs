using AutoMapper;
using GameStore.Core.Dto.Requests.Order;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Payment;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Payment.Options.PaymentGateway;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Errors;
using GameStore.Models.Exceptions;
using GameStore.Models.Results;
using Microsoft.Extensions.Options;

namespace GameStore.Core.Services
{
    internal class PaymentService(
        IOptionsSnapshot<PaymentGatewayOptions> paymentGatewayOptions,
        IPaymentMethodFactory paymentMethodFactory,
        IUnitOfWork gameStoreUnitOfWork,
        IMapper mapper
        ) : IPaymentService
    {
        public IEnumerable<PaymentMethodResponse> GetPaymentMethods()
        {
            return mapper.Map<IEnumerable<PaymentMethodResponse>>(paymentGatewayOptions.Value.PaymentMethods);
        }

        public async Task<Result<IPaymentResponse>> ProcessPayment(OrderPaymentRequest request, Guid customerId, CancellationToken token = default)
        {
            IPaymentMethod paymentMethod = paymentMethodFactory.GetStrategy(request.Method);

            var order = await gameStoreUnitOfWork.Orders.GetCartByCustomerIdAsync(customerId, token);
            if (order is null)
            {
                return Result.Failure<IPaymentResponse>(OrderErrors.NotFound);
            }

            if (order.Status != OrderStatus.Open)
            {
                return Result.Failure<IPaymentResponse>(OrderErrors.AlreadyProcessedOrder);
            }

            var reservedGames = new List<(Game Game, int Quantity)>();
            try
            {
                foreach (var item in order.OrderProducts)
                {
                    var game = await gameStoreUnitOfWork.Games.GetOrMigrateByKeyAsync(item.ProductKey, token);

                    if (game == null)
                    {
                        return Result.Failure<IPaymentResponse>(GameErrors.NotFound);
                    }

                    if (game.UnitInStock < item.Quantity)
                    {
                        return Result.Failure<IPaymentResponse>(GameErrors.OutOfStock);
                    }

                    var result = game.ReduceUnitsInStock(item.Quantity);
                    if (result.IsFailure)
                    {
                        return Result.Failure<IPaymentResponse>(result.Error);
                    }

                    reservedGames.Add((game, item.Quantity));
                }

                await gameStoreUnitOfWork.SaveChangesAsync(token);
            }
            catch (UpdateConcurrencyException)
            {
                return Result.Failure<IPaymentResponse>(GameErrors.ConcurrentUpdate);
            }

            try
            {
                var paymentResult = await paymentMethod.Process(order, request.Model);

                if (paymentResult.IsFailure)
                {
                    order.Cancelled();
                    CompensateReservedProductsAsync(reservedGames);

                    await gameStoreUnitOfWork.SaveChangesAsync(token);
                    return paymentResult;
                }

                order.Paid(request.Method);
                await gameStoreUnitOfWork.SaveChangesAsync(token);
                return paymentResult;
            }
            catch (Exception)
            {
                order.Cancelled();
                CompensateReservedProductsAsync(reservedGames);

                await gameStoreUnitOfWork.SaveChangesAsync(token);

                return Result.Failure<IPaymentResponse>(PaymentErrors.UnknownPaymentFailure);
            }
        }

        private static void CompensateReservedProductsAsync(List<(Game Game, int Quantity)> reservedGames)
        {
            foreach (var reservation in reservedGames)
            {
                reservation.Game.AddUnitsInStock(reservation.Quantity);
            }
        }
    }
}