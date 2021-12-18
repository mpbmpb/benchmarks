using System.Security.Cryptography;

namespace Benchmarks;

public static class DotNetCryptoHelper
{
    private const int SaltSize = 128 / 8; // number of bits expressed in bytes
    public static byte[] GetSalt(int bitLength) => RandomNumberGenerator.GetBytes(bitLength / 8);
    public static byte[] GetSalt() => RandomNumberGenerator.GetBytes(SaltSize);

    public static string HashPassword512(string password, byte[] salt, int iterations)
    {
        var outputLength = 512 / 8; // number of bits expressed in bytes
        byte[] result = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA512, outputLength); 
        return Convert.ToBase64String(result);
    }
    public static string HashPassword256(string password, byte[] salt, int iterations)
    {
        var outputLength = 256 / 8; // number of bits expressed in bytes
        byte[] result = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, outputLength); 
        return Convert.ToBase64String(result);
    }
}