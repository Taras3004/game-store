using Microsoft.AspNetCore.Http;

namespace GameStore.Payment.Functions.Exceptions;

public abstract class PaymentException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public sealed class PaymentValidationException(string message)
    : PaymentException(message, StatusCodes.Status400BadRequest);

public sealed class TransactionDeclinedException(string message)
    : PaymentException(message, StatusCodes.Status402PaymentRequired);

public static class ErrorMessages
{
    public const string ExpirationDateCanNotBeInPast = "Expiration year and month can not be in the past.";
    public const string NotEnoughMoney = "Not enough money to pay for your order.";
    public const string IncorrectCardNumberLength = "Card number must contain 16 digits.";
    public const string IncorrectCardNumberPaymentSystem = "Visa card number must start with 4.";
}
