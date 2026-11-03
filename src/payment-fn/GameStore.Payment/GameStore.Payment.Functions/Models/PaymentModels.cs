using System.ComponentModel.DataAnnotations;

namespace GameStore.Payment.Functions.Models;

public enum PaymentMethod
{
    Visa,
    IBox,
    Bank,
}

public abstract class TransactionRequestBase
{
    [Range(0.0, double.MaxValue)]
    public decimal TransactionAmount { get; init; }
}

public sealed class VisaTransactionRequest : TransactionRequestBase
{
    [Required]
    public string CardHolderName { get; init; } = string.Empty;

    [Required]
    public string CardNumber { get; init; } = string.Empty;

    [Range(1, 12)]
    public int ExpirationMonth { get; init; }

    [Range(1, 999)]
    public int Cvv { get; init; }

    [Range(1960, int.MaxValue)]
    public int ExpirationYear { get; init; }
}

public sealed class IBoxTransactionRequest : TransactionRequestBase
{
    public Guid AccountNumber { get; init; }

    public Guid InvoiceNumber { get; init; }
}

public sealed record IBoxPaymentResponse(
    Guid AccountNumber,
    Guid InvoiceNumber,
    decimal Amount,
    PaymentMethod PaymentMethod,
    string AccountId);

public sealed record ErrorResponse(string Title, int Status);

public sealed class Account
{
    public string Id { get; set; } = string.Empty;

    public decimal Money { get; set; }
}
