using System.Text;
using Konscious.Security.Cryptography;

namespace Benchmarks;

public static class Argon2Helper
{
    public static string HashPassword(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

        argon2.Salt = salt;
        argon2.DegreeOfParallelism = 2; // value / 2 = number of cores
        argon2.Iterations = 2;
        argon2.MemorySize = 512 * 1024; // 0.5 GB

        return Convert.ToBase64String(argon2.GetBytes(16));
    }
    
    public static string HashPassword(string password, byte[] salt, int parallelism, int iterations, int memSizeInMB)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

        argon2.Salt = salt;
        argon2.DegreeOfParallelism = parallelism; // value / 2 = number of cores
        argon2.Iterations = iterations;
        argon2.MemorySize = memSizeInMB * 1024;

        return Convert.ToBase64String(argon2.GetBytes(16));
    }

    public static bool Verify(string password, byte[] salt, string hash)
    {
        var oldHash = hash;
        var newHash = HashPassword(password, salt);

        // purposefully using NOT-optimized linq to defend against timing attacks
        return newHash.Select((val, index) => (val, index)).Count(c => c.val != oldHash[c.index]) == 0;
    }
    
    public static bool Verify(string password, byte[] salt, string hash, int parallelism, int iterations, int memSizeInMB)
    {
        var oldHash = hash;
        var newHash = HashPassword(password, salt, parallelism, iterations, memSizeInMB);

        // purposefully using NOT-optimized linq to defend against timing attacks
        return newHash.Select((val, index) => (val, index)).Count(c => c.val != oldHash[c.index]) == 0;
    }
    
}