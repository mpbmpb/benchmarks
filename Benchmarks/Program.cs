using Benchmarks;

// BenchmarkRunner.Run<ForEachTests>();

// BenchmarkRunner.Run<RegexTests>();

// BenchmarkRunner.Run<HashMethodsTests>();

BenchmarkRunner.Run<StringConcatTests>();

// var test = new StringConcatTests();
// test.NumberOfConcats = 4;
// test.ChunkSize = 10;
//
// void pad() => Console.WriteLine("\n".PadLeft(Console.WindowWidth,'-'));
//
// Console.WriteLine();
// pad();
// Console.WriteLine(test.StringConcat());
// pad();
// Console.WriteLine(test.StringBuilderMethod());
// pad();
// Console.WriteLine(test.StringConcatWithChars());
// pad();
// Console.WriteLine(test.SpanMethodReturningFromArray());
// pad();
// Console.WriteLine(test.SpanMethodReturningFromSpan());
// pad();
// Console.WriteLine(test.SpanMethodReturningFromSpanToString());
// pad();
// Console.WriteLine("stringBuilder with char");
// Console.WriteLine(test.StringBuilderMethodWithChars());
// pad();
// Console.WriteLine("stackalloc");
// Console.WriteLine(test.SpanMethodStackalloc());
// pad();
// Console.WriteLine("stackalloc toString");
// Console.WriteLine(test.SpanMethodStackallocToString());
// pad();
// Console.WriteLine("all span");
// Console.WriteLine(test.AllSpanMethod());
// pad();
// Console.WriteLine("all span stackalloc");
// Console.WriteLine(test.AllSpanMethodStackalloc());
// pad();
// Console.WriteLine("pointer method");
// Console.WriteLine(test.PointerMethod());
// pad();
// Console.WriteLine("AllPointerMethod");
// Console.WriteLine(test.AllPointerMethod());
// ;