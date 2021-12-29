using Benchmarks;

// BenchmarkRunner.Run<ForEachTests>();

// BenchmarkRunner.Run<RegexTests>();

// BenchmarkRunner.Run<HashMethodsTests>();

BenchmarkRunner.Run<StringConcatTests>();
//
// var test = new StringConcatTests();
// test.NumberOfConcats = 100;
// test.ChunkSize = 1000;
// test.WithCharArray = false;
//
// void pad() => Console.WriteLine("\n".PadLeft(Console.WindowWidth,'-'));
//
// Console.WriteLine();
// pad();
// Console.WriteLine(nameof(test.WithOperator));
// Console.WriteLine(test.WithOperator());
// pad();
// Console.WriteLine(nameof(test.WithStringBuilder));
// Console.WriteLine(test.WithStringBuilder());
// pad();
// Console.WriteLine(nameof(test.WithSpan));
// Console.WriteLine(test.WithSpan());
// pad();
// Console.WriteLine(nameof(test.WithSpanStackalloc));
// Console.WriteLine(test.WithSpanStackalloc());
// pad();
// Console.WriteLine(nameof(test.WithPointers));
// Console.WriteLine(test.WithPointers());
// pad();
// ;