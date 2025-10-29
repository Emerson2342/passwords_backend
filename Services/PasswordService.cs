using System.Security.Cryptography;
namespace passwords_backend.Services;

public static class PasswordService
{
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        byte[] hashBytes = new byte[48];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
        Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }


    public static bool VerifyPassword(string password, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        byte[] salt = new byte[16];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

        byte[] storedSubKey = new byte[32];
        Buffer.BlockCopy(hashBytes, 16, storedSubKey, 0, 32);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] computedSubKey = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(storedSubKey, computedSubKey);

    }

}