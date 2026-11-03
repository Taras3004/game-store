namespace GameStore.Payment.Functions.Options;

public sealed class PaymentOptions
{
    public const string SectionName = "Payment";

    /// <summary>
    /// Delay applied to roughly 10% of transactions to simulate an unreliable payment provider.
    /// The original service read it from the "TimeoutInSeconds" setting, which defaulted to 0.
    /// </summary>
    public int TimeoutInSeconds { get; set; }

    /// <summary>
    /// Balance assigned to an account that does not exist yet and is created on first payment.
    /// </summary>
    public decimal DefaultAccountMoney { get; set; }

    /// <summary>
    /// Accounts storage file. Relative paths resolve against the application base directory.
    /// </summary>
    public string AccountsFilePath { get; set; } = "account.json";
}
