namespace Benchmarks;

[MemoryDiagnoser(false)]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class HashMethodsTests
{
    private const int workload = 11;
    private const string password = "My secret password";

    [Benchmark]
    public void Pbdkf2_with_SHA256()
    {
        var salt = DotNetCryptoHelper.GetSalt();
        var hash = DotNetCryptoHelper.HashPassword256(password, salt, 10000);
    }

    [Benchmark]
    public void Pbdkf2_with_SHA512()
    {
        var salt = DotNetCryptoHelper.GetSalt();
        var hash = DotNetCryptoHelper.HashPassword512(password, salt, 10000);
    }

    [Benchmark]
    public void BCrypt_standard()
    {
        var salt = BCryptHelper.GetSalt(workload);
        var hash = BCryptHelper.HashPassword(password, salt);
    }

    [Benchmark]
    public void BCrypt_Enhanced()
    {
        var hash = BCryptHelper.EnhanceHashPassword(password, workload);
    }
    
    [Benchmark]
    public void BCrypt_enhanced_512()
    {
        var hash = BCryptHelper.EnhanceHashPassword512(password, workload);
    }

    [Benchmark]
    [Arguments(2,2, 512)]
    [Arguments(4,2, 512)]
    [Arguments(8,2, 512)]
    [Arguments(8,4, 512)]
    public void Argon2(int parallelism, int iterations, int memSizeMB)
    {
        var salt = DotNetCryptoHelper.GetSalt();
        var hash = Argon2Helper.HashPassword(password, salt, parallelism, iterations, memSizeMB);
    }
}