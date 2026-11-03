using GameStore.Core.Dto.Details.Payment;
using GameStore.Models.Entities;
using GameStore.Models.Results;

namespace GameStore.Core.Interfaces.Payment
{
    public interface IPaymentMethod
    {
        public Task<Result<IPaymentResponse>> Process(Order order, PaymentModel? model = null);
    }
}
