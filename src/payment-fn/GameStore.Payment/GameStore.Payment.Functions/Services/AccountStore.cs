using System.Text.Json;
using GameStore.Payment.Functions.Models;
using GameStore.Payment.Functions.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Payment.Functions.Services;

/// <summary>
/// File-backed account balances (account.json), same storage format as the original microservice.
/// </summary>
public sealed class AccountStore(IOptions<PaymentOptions> options)
{
    private static readonly JsonSerializerOptions ReadOptions = new() { PropertyNameCaseInsensitive = true };
    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    private readonly SemaphoreSlim _lock = new(1, 1);

    private string FilePath
    {
        get
        {
            var path = options.Value.AccountsFilePath;
            return Path.IsPathRooted(path) ? path : Path.Combine(AppContext.BaseDirectory, path);
        }
    }

    public async Task<decimal> GetOrCreateBalanceAsync(string accountId, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var accounts = await ReadAllAsync(cancellationToken);
            var account = accounts.Find(a => a.Id == accountId);
            if (account is not null)
            {
                return account.Money;
            }

            account = new Account { Id = accountId, Money = options.Value.DefaultAccountMoney };
            accounts.Add(account);
            await WriteAllAsync(accounts, cancellationToken);
            return account.Money;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> TryWithdrawAsync(string accountId, decimal amount, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var accounts = await ReadAllAsync(cancellationToken);
            var account = accounts.Find(a => a.Id == accountId);
            if (account is null || amount > account.Money)
            {
                return false;
            }

            account.Money -= amount;
            await WriteAllAsync(accounts, cancellationToken);
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<Account>> ReadAllAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        await using var stream = File.OpenRead(FilePath);
        if (stream.Length == 0)
        {
            return [];
        }

        return await JsonSerializer.DeserializeAsync<List<Account>>(stream, ReadOptions, cancellationToken) ?? [];
    }

    private async Task WriteAllAsync(List<Account> accounts, CancellationToken cancellationToken)
    {
        await using var stream = File.Create(FilePath);
        await JsonSerializer.SerializeAsync(stream, accounts, WriteOptions, cancellationToken);
    }
}
