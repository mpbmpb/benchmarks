using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class StringConcatTests
{
    private readonly Random _random = new Random();
    
    [Params(4, 10, 100)]
    public int NumberOfConcats { get; set; } = 2;
    
    public int ChunkSize { get; set; } = 10;

    public char RandomChar => (char)_random.Next(65, 122);
    public char[] RandomChars() => Enumerable.Range(0, ChunkSize).Select(
        _ => (char)_random.Next(65, 122)).ToArray();

    public string RandomString() => new string(RandomChars());


    [Benchmark]
    public string StringConcat()
    {
        var str = "";
        for (int i = 0; i < NumberOfConcats; i++)
        {
            str += String.Concat(RandomString());
        }

        return str;
    }

     [Benchmark]
    public string StringConcatWithOperator()
    {
        var str = "";
        for (int i = 0; i < NumberOfConcats; i++)
        {
            str += RandomString();
        }

        return str;
    }

     [Benchmark]
    public string StringConcatWithChars()
    {
        var str = "";
        for (int i = 0; i < NumberOfConcats; i++)
        {
            str += String.Concat(RandomChars());
        }

        return str;
    }

    [Benchmark]
    public string StringBuilderMethod()
    {
        var builder = new StringBuilder();

        for (int i = 0; i < NumberOfConcats; i++)
        {
            builder.Append(RandomString());
        }

        return builder.ToString();
    }
    
    [Benchmark]
    public string StringBuilderMethodWithChars()
    {
        var builder = new StringBuilder();

        for (int i = 0; i < NumberOfConcats; i++)
        {
            builder.Append(RandomChars());
        }

        return builder.ToString();
    }

    [Benchmark]
    public string SpanMethodReturningFromArray()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        var span = new Span<char>(array);

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return new String(array);
    }
    
    [Benchmark]
    public string SpanMethodReturningFromSpan()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        var span = new Span<char>(array);

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return new String(span);
    }
    
    [Benchmark]
    public string SpanMethodReturningFromSpanToString()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        var span = new Span<char>(array);

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return span.ToString();
    }
    
    [Benchmark]
    public string SpanMethodStackalloc()
    {
        Span<char> span = stackalloc char[NumberOfConcats * ChunkSize];

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return new String(span);
    }

    [Benchmark]
    public string SpanMethodStackallocToString()
    {
        Span<char> span = stackalloc char[NumberOfConcats * ChunkSize];

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return span.ToString();
    }

    [Benchmark]
    public string AllSpanMethod()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        var span = new Span<char>(array);

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString().AsSpan();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return span.ToString();
    }

    [Benchmark]
    public string AllSpanMethodStackalloc()
    {
        Span<char> span = stackalloc char[NumberOfConcats * ChunkSize];

        for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
        {
            var rndString = RandomString().AsSpan();
            
            for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
            {
                var index = concatNo * ChunkSize + currentChar;
                span[index] = rndString[currentChar];
            }
        }

        return span.ToString();
    }

    [Benchmark]
    public string PointerMethod()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        
        unsafe
        {
            fixed (char* pointer = &array[0])
            {
                var index = pointer;
                for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
                {
                    var rndString = RandomString();
                        for (int currentChar = 0; currentChar < ChunkSize; currentChar++)
                        {
                            *index = rndString[currentChar];
                            index++;
                        }
                }
            }
        }

        return new String(array);
    }
    
    [Benchmark]
    public string AllPointerMethod()
    {
        var array = new char[NumberOfConcats * ChunkSize];
        
        unsafe
        {
            fixed (char* pointer = &array[0])
            {
                var index = pointer;
                for (int concatNo = 0; concatNo < NumberOfConcats; concatNo++)
                {
                    var rndString = RandomString();
                    fixed (char* strPointer = rndString)
                    {
                        var current = strPointer;
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