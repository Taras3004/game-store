using GameStore.Core.Dto.Requests.Order;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces.Payment;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethodResponse> GetPaymentMethods();

        Task<Result<IPaymentResponse>> ProcessPayment(OrderPaymentRequest request, Guid customerId, CancellationToken token = default);
    }
}
