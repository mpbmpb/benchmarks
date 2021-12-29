using System.Collections.Immutable;
using System.Text;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace Benchmarks;

[MemoryDiagnoser]
[Config(typeof(Config))]
[RankColumn]
public class StringConcatTests
{
    private class Config : ManualConfig
    {
        public Config() => Orderer = new FastestToSlowestOrderer();

        private class FastestToSlowestOrderer : IOrderer
        {
            public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) =>
                benchmarksCase.OrderBy(benchmark => benchmark.Descriptor.WorkloadMethodDisplayInfo);

            public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary) =>
                benchmarksCase.OrderBy(benchmark => benchmark.Parameters[nameof(ChunkSize)])
                    .ThenBy(benchmark => benchmark.Parameters[nameof(NumberOfConcats)])
                    .ThenByDescending(benchmark => benchmark.Parameters[nameof(WithCharArray)])
                    .ThenBy(benchmark => summary[benchmark].ResultStatistics.Mean);

            public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => null!;

            public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) =>
                benchmarkCase.Job.DisplayInfo + "_" + benchmarkCase.Parameters.DisplayInfo;

            public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) =>
                logicalGroups.OrderBy(it => it.Key);

            public bool SeparateLogicalGroups => true;
        }
    }
    private readonly Random _random = new Random();
    
    [Params(4, 100)]
    public int NumberOfConcats { get; set; } = 2;
    
    [ParamsAllValues]
    public bool WithCharArray { get; set; }
    
    [Params(10, 1000)]
    public int ChunkSize { get; set; } = 10;

    public char RandomChar => (char)_random.Next(65, 122);
    public char[] RandomChars() => Enumerable.Range(0, ChunkSize).Select(
        _ => (char)_random.Next(65, 122)).ToArray();

    public string RandomString() => new string(RandomChars());

    
     [Benchmark]
    public string WithOperator()
    {
        var str = "";
        for (int i = 0; i < NumberOfConcats; i++)
        {
            str += WithCharArray ? String.Concat(RandomChars()) : RandomString();
        }

        return str;
    }

    [Benchmark]
    public string WithStringBuilder()
    {
        var builder = new StringBuilder();

        for (int i = 0; i < NumberOfConcats; i++)
        {
            if (WithCharArray)
                builder.Append(RandomChars());
            else
                builder.Append(RandomString());
        }

        return builder.ToString();
    }
    
    [Benchmark]
    public string WithSpan()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        var span = new Span<char>(array);

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = WithCharArray ? null : RandomString();
            var rndChars = WithCharArray ? RandomChars() : Array.Empty<char>();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = WithCharArray ? rndChars[currentChar] : rndString![currentChar];
            }
        }

        return new String(array);
    }

    [Benchmark]
    public string WithSpanStackalloc()
    {
        Span<char> span = stackalloc char[NumberOfConcats * ChunkSize];

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = WithCharArray ? null : RandomString();
            var rndChars = WithCharArray ? RandomChars() : Array.Empty<char>();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = WithCharArray ? rndChars[currentChar] : rndString![currentChar];
            }
        }

        return new String(span);
    }
    
    [Benchmark]
    public string WithPointers()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        
        unsafe
        {
            fixed (char* pointer = &array[0])
            {
                var index = pointer;
                for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
                {
                    var rndString = WithCharArray ? null : RandomString();
                    var rndChars = WithCharArray ? RandomChars() : Array.Empty<char>();
                    
                    fixed (char* strPointer =  rndString, chrPointer = rndChars)
                    {
                        var current = WithCharArray ? chrPointer : strPointer;
                        for (int i = 0; i < ChunkSize; i++)
                        {
                            *index = *current;
                            index++;
                            current++;
                        }
                    }
                }
            }
        }
        
        return new String(array);
    }
    
}