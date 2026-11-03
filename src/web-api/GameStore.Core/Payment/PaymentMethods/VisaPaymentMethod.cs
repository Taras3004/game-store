using GameStore.Core.Dto.Details.Payment;
using GameStore.Core.Interfaces.Payment;
using GameStore.Models.Constants;
using GameStore.Models.Entities;
using System.Net.Http.Json;
using GameStore.Core.Dto.Responses.Payment.Microservice;
using GameStore.Core.Dto.Responses.Payment.PaymentResults;
using GameStore.Models.Errors;
using GameStore.Models.Results;

namespace GameStore.Core.Payment.PaymentMethods
{
    internal class VisaPaymentMethod(IHttpClientFactory httpClientFactory) : IPaymentMethod
    {
        public Task<Result<IPaymentResponse>> Process(Order order, PaymentModel? model = null)
        {
            ArgumentNullException.ThrowIfNull(order);
            ArgumentNullException.ThrowIfNull(model);

            return ProcessAsync(order, model);
        }

        private async Task<Result<IPaymentResponse>> ProcessAsync(Order order, PaymentModel model)
        {
            var httpClient = httpClientFactory.CreateClient(HttpClients.PaymentHttpClientName);

            var payload = new
            {
                TransactionAmount = order.CalculateTransactionAmount(),
                CardHolderName = model.Holder,
                model.CardNumber,
                ExpirationMonth = model.MonthExpire,
                Cvv = model.Cvv2,
                ExpirationYear = model.YearExpire
            };

            var response = await httpClient.PostAsJsonAsync("payments/visa", payload);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<VisaErrorResponse>();
                return errorResponse is null ?
                    Result.Failure<IPaymentResponse>(PaymentErrors.UnknownPaymentFailure) :
                    Result.Failure<IPaymentResponse>(
                        new Error("Visa.PaymentFailure", errorResponse.Title, ErrorType.Failure));
            }

            return new VisaPaymentResponse();
        }
    }
}
