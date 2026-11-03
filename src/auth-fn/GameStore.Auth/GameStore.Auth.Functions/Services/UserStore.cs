using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GameStore.Auth.Functions.Models;
using GameStore.Auth.Functions.Options;
using Microsoft.Extensions.Options;

namespace GameStore.Auth.Functions.Services;

public sealed class UserStoreException(string message) : Exception(message);

/// <summary>
/// Users stored as JSON, AES-256-CBC (PKCS7) encrypted and base64 encoded, matching the original AuthService file format.
/// </summary>
public sealed class UserStore
{
    private static readonly JsonSerializerOptions ReadOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _filePath;
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public UserStore(IOptions<UserStorageOptions> options)
    {
        var value = options.Value;
        _filePath = Path.IsPathRooted(value.FilePath)
            ? value.FilePath
            : Path.Combine(AppContext.BaseDirectory, value.FilePath);
        _key = Convert.FromBase64String(value.Key);
        _iv = Convert.FromBase64String(value.IV);
    }

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken) =>
        WithLockAsync<IReadOnlyList<User>>(async () => await ReadAllAsync(cancellationToken), cancellationToken);

    /// <summary>
    /// Email and password are compared exactly (case-sensitive), as in the original service.
    /// </summary>
    public async Task<User?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        var users = await GetAllAsync(cancellationToken);
        return users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken) =>
        WithLockAsync(async () =>
        {
            var users = await ReadAllAsync(cancellationToken);
            if (users.Any(u => EmailEquals(u.Email, user.Email)))
            {
                throw new UserStoreException($"User with email {user.Email} already exists");
            }

            users.Add(user);
            await WriteAllAsync(users, cancellationToken);
            return true;
        }, cancellationToken);

    public Task UpdateAsync(User user, string? originalEmail, CancellationToken cancellationToken) =>
        WithLockAsync(async () =>
        {
            var users = await ReadAllAsync(cancellationToken);
            if (users.Any(u => EmailEquals(u.Email, user.Email) && !EmailEquals(u.Email, originalEmail)))
            {
                throw new UserStoreException($"User with email {user.Email} already exists");
            }

            var existing = users.FirstOrDefault(u => EmailEquals(u.Email, originalEmail))
                ?? throw new UserStoreException($"User with email {originalEmail} is not found");

            users.Remove(existing);
            users.Add(user);
            await WriteAllAsync(users, cancellationToken);
            return true;
        }, cancellationToken);

    public Task DeleteAsync(string email, CancellationToken cancellationToken) =>
        WithLockAsync(async () =>
        {
            var users = await ReadAllAsync(cancellationToken);
            var existing = users.FirstOrDefault(u => EmailEquals(u.Email, email))
                ?? throw new UserStoreException($"User with email {email} is not found");

            users.Remove(existing);
            await WriteAllAsync(users, cancellationToken);
            return true;
        }, cancellationToken);

    private static bool EmailEquals(string left, string? right) =>
        string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);

    private async Task<T> WithLockAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return await action();
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<User>> ReadAllAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        var content = (await File.ReadAllTextAsync(_filePath, cancellationToken)).Trim();
        if (content.Length == 0)
        {
            return [];
        }

        using var aes = CreateAes();
        var plain = aes.DecryptCbc(Convert.FromBase64String(content), _iv);
        return JsonSerializer.Deserialize<List<User>>(plain, ReadOptions) ?? [];
    }

    private async Task WriteAllAsync(List<User> users, CancellationToken cancellationToken)
    {
        using var aes = CreateAes();
        var encrypted = aes.EncryptCbc(JsonSerializer.SerializeToUtf8Bytes(users), _iv);
        await File.WriteAllTextAsync(_filePath, Convert.ToBase64String(encrypted), Encoding.UTF8, cancellationToken);
    }

    private Aes CreateAes()
    {
        var aes = Aes.Create();
        aes.Key = _key;
        return aes;
    }
}
