using BCrypt.Net;
using BC = BCrypt.Net.BCrypt ;

namespace Benchmarks;

public static class BCryptHelper
{
    public static string GetSalt() => BC.GenerateSalt();
    public static string GetSalt(int workload) => BC.GenerateSalt(workload);
    public static string? HashPassword(string password, string salt) 
        => BC.HashPassword(password, salt);

    public static string EnhanceHashPassword(string password, int workload)
        => BC.EnhancedHashPassword(password,workload);
    
    public static string EnhanceHashPassword512(string password, int workload)
        => BC.EnhancedHashPassword(password, HashType.SHA512,workload);
}