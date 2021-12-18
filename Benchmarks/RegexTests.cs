using System.Text.RegularExpressions;
using Bogus;

namespace Benchmarks;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class RegexTests
{
    private static readonly Regex _regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

    private static Regex _regexCompiled = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    private static readonly List<Customer> _customers = new Faker<Customer>()
        .RuleFor(c => c.id, f => f.IndexFaker)
        .RuleFor(c => c.FirstName, f => f.Person.FirstName)
        .RuleFor(c => c.LastName, f => f.Person.LastName)
        .RuleFor(c => c.Email, f => f.Person.Email)
        .Generate(10);

    private static readonly List<string> _emails = _customers.Select(c => c.Email ?? "").ToList();

    [Benchmark]
    public void IsMatch()
    {
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

        foreach (var email in _emails)
        {
            var isValid = regex.IsMatch(email);
        }
    }
    
    [Benchmark]
    public void IsMatchCompiled()
    {
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        foreach (var email in _emails)
        {
            var isValid = regex.IsMatch(email);
        }
    }

    [Benchmark]
    public void IsMatchCached()
    {
        foreach (var email in _emails)
        {
            var isValid = _regex.IsMatch(email);
        }
    }

    [Benchmark]
    public void IsMatchCachedCompiled()
    {
        foreach (var email in _emails)
        {
            var isValid = _regexCompiled.IsMatch(email);
        }
    }

    
}

public class Customer
{
    public int id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}