using GameStore.Payment.Functions.Exceptions;
using GameStore.Payment.Functions.Http;
using GameStore.Payment.Functions.Models;
using GameStore.Payment.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GameStore.Payment.Functions.Functions;

/// <summary>
/// POST api/payments/visa and POST api/payments/ibox (routes prefixed with "api" by the Functions host).
/// </summary>
public sealed class PaymentsFunctions(PaymentProcessor processor, ILogger<PaymentsFunctions> logger)
{
    [Function("PayVisa")]
    public async Task<IActionResult> PayVisa(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "payments/visa")] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var body = await RequestBody.ReadAsync<VisaTransactionRequest>(request, cancellationToken);
        if (body.Error is not null)
        {
            return body.Error;
        }

        return await ExecuteAsync(async () =>
        {
            await processor.PayVisaAsync(body.Model!, cancellationToken);
            return new OkResult();
        });
    }

    [Function("PayIBox")]
    public async Task<IActionResult> PayIBox(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "payments/ibox")] HttpRequest request,
        CancellationToken cancellationToken)
    {
        var body = await RequestBody.ReadAsync<IBoxTransactionRequest>(request, cancellationToken);
        if (body.Error is not null)
        {
            return body.Error;
        }

        return await ExecuteAsync(async () =>
            new OkObjectResult(await processor.PayIBoxAsync(body.Model!, cancellationToken)));
    }

    /// <summary>
    /// Maps exceptions to { "title", "status" } bodies, like the original ExceptionMiddleware.
    /// </summary>
    private async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (PaymentException ex)
        {
            return Error(ex.Message, ex.StatusCode);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Unhandled payment error");
            return Error(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private static ObjectResult Error(string title, int status) =>
        new(new ErrorResponse(title, status)) { StatusCode = status };
}
