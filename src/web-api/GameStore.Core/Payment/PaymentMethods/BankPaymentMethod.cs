using GameStore.Core.Dto.Responses.Payment.PaymentResults;
using GameStore.Core.Interfaces.Payment;
using GameStore.Models.Entities;
using GameStore.Models.Results;
using Microsoft.Extensions.Options;
using System.Text;
using GameStore.Core.Dto.Details.Payment;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Payment.Options;

namespace GameStore.Core.Payment.PaymentMethods
{
    internal class BankPaymentMethod(IOptionsSnapshot<PaymentOptions> paymentSettings, IDateTimeProvider dateTimeProvider) : IPaymentMethod
    {
        public Task<Result<IPaymentResponse>> Process(Order order, PaymentModel? model = null)
        {
            var datetime = dateTimeProvider.UtcNow;

            string invoiceContent = $"Customer Id: {order.CustomerId}\n" +
                                    $"Order Id: {order.Id}\n" +
                                    $"Date: {datetime:yyyy-MM-dd}\n" +
                                    $"Valid until: {datetime.AddDays(paymentSettings.Value.PaymentValidityDurationDays)}\n" +
                                    $"Transaction amount: {order.CalculateTransactionAmount()}\n";

            byte[] fileBytes = Encoding.UTF8.GetBytes(invoiceContent);

            var bankResult = new BankPaymentResponse
            {
                FileContent = fileBytes,
                ContentType = "application/pdf",
                FileName = $"invoice_{order.Id}.pdf"
            };

            return Task.FromResult<Result<IPaymentResponse>>(bankResult);
        }
    }
}
