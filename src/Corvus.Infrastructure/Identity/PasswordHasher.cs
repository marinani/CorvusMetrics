using System.Security.Cryptography;
using Corvus.Application.Authentication.Abstractions;

namespace Corvus.Infrastructure.Identity;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        var hash = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hash, 0, SaltSize);
        Array.Copy(key, 0, hash, SaltSize, KeySize);

        return Convert.ToBase64String(hash);
    }

    public bool Verify(string password, string passwordHash)
    {
        var hashBytes = Convert.FromBase64String(passwordHash);

        if (hashBytes.Length != SaltSize + KeySize)
        {
            return false;
        }

        var salt = hashBytes[..SaltSize];
        var storedKey = hashBytes[SaltSize..];

        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return CryptographicOperations.FixedTimeEquals(storedKey, key);
    }
}