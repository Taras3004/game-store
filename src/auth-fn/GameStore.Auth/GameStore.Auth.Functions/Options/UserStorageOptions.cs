namespace GameStore.Auth.Functions.Options;

public sealed class UserStorageOptions
{
    public const string SectionName = "UserStorage";

    /// <summary>
    /// Encrypted users file. Relative paths resolve against the application base directory.
    /// </summary>
    public string FilePath { get; set; } = "DefinetlyNotUsers.txt";

    /// <summary>
    /// AES-256 key (base64). The default is the key the original AuthService used, so the existing file still decrypts.
    /// </summary>
    public string Key { get; set; } = "m/1W1CR79h6W818KpeO4Io4YvgJeere2qIjRArbbwKQ=";

    /// <summary>
    /// AES CBC initialization vector (base64). The default matches the original AuthService.
    /// </summary>
    public string IV { get; set; } = "FjvssO05dfGrcaaHFiYnhQ==";
}
