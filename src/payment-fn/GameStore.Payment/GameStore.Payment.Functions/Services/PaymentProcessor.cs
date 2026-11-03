using GameStore.Payment.Functions.Exceptions;
using GameStore.Payment.Functions.Models;
using GameStore.Payment.Functions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameStore.Payment.Functions.Services;

public sealed class PaymentProcessor(
    AccountStore accounts,
    IOptions<PaymentOptions> options,
    TimeProvider timeProvider,
    ILogger<PaymentProcessor> logger)
{
    private const char VisaDesignation = '4';
    private const int CardNumberLength = 16;

    public Task PayVisaAsync(VisaTransactionRequest request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        if (request.ExpirationYear < now.Year ||
            (request.ExpirationYear == now.Year && request.ExpirationMonth < now.Month))
        {
            throw new PaymentValidationException(ErrorMessages.ExpirationDateCanNotBeInPast);
        }

        if (request.CardNumber.Length != CardNumberLength)
        {
            throw new PaymentValidationException(ErrorMessages.IncorrectCardNumberLength);
        }

        if (request.CardNumber[0] != VisaDesignation)
        {
            throw new PaymentValidationException(ErrorMessages.IncorrectCardNumberPaymentSystem);
        }

        return WithdrawAsync(request.CardNumber, request.TransactionAmount, cancellationToken);
    }

    public async Task<IBoxPaymentResponse> PayIBoxAsync(IBoxTransactionRequest request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountNumber.ToString();
        await WithdrawAsync(accountId, request.TransactionAmount, cancellationToken);

        return new IBoxPaymentResponse(
            request.AccountNumber,
            request.InvoiceNumber,
            request.TransactionAmount,
            PaymentMethod.IBox,
            accountId);
    }

    private async Task WithdrawAsync(string accountId, decimal amount, CancellationToken cancellationToken)
    {
        var balance = await accounts.GetOrCreateBalanceAsync(accountId, cancellationToken);
        if (amount > balance)
        {
            throw new TransactionDeclinedException(ErrorMessages.NotEnoughMoney);
        }

        // The original service stalled ~10% of requests to simulate an unreliable provider.
        var timeout = options.Value.TimeoutInSeconds;
        if (timeout > 0 && Random.Shared.Next(0, 10) == 0)
        {
            logger.LogInformation("Simulating provider delay of {Timeout}s for account {AccountId}", timeout, accountId);
            await Task.Delay(TimeSpan.FromSeconds(timeout), cancellationToken);
        }

        if (!await accounts.TryWithdrawAsync(accountId, amount, cancellationToken))
        {
            throw new TransactionDeclinedException(ErrorMessages.NotEnoughMoney);
        }
    }
}
