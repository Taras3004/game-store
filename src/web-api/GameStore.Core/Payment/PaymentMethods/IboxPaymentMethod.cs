using GameStore.Core.Interfaces.Payment;
using GameStore.Models.Constants;
using GameStore.Models.Entities;
using GameStore.Models.Results;
using System.Net.Http.Json;
using GameStore.Core.Dto.Details.Payment;
using GameStore.Core.Dto.Responses.Payment.PaymentResults;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Errors;

namespace GameStore.Core.Payment.PaymentMethods
{
    internal class IboxPaymentMethod(IHttpClientFactory httpClientFactory, IDateTimeProvider dateTimeProvider) : IPaymentMethod
    {
        public async Task<Result<IPaymentResponse>> Process(Order order, PaymentModel? model = null)
        {
            var httpClient = httpClientFactory.CreateClient(HttpClients.PaymentHttpClientName);

            var transactionAmount = order.CalculateTransactionAmount();

            var payload = new
            {
                AccountNumber = order.CustomerId,
                InvoiceNumber = order.Id,
                TransactionAmount = transactionAmount
            };

            var response = await httpClient.PostAsJsonAsync("payments/ibox", payload);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<IPaymentResponse>(PaymentErrors.UnknownPaymentFailure);
            }

            DateTime currentTime = dateTimeProvider.UtcNow;

            var iboxResult = new IboxPaymentResponse
            {
                UserId = order.CustomerId,
                OrderId = order.Id,
                PaymentDate = currentTime,
                Sum = transactionAmount,
            };

            return iboxResult;
        }
    }
}
