using System;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var sw = Stopwatch.StartNew();
        var fileName = new Generator().Generate(500_000);
        new Sorter().Sort(fileName, 50_000);
        sw.Stop();
        Console.WriteLine($"Execution took: {sw.Elapsed}");
    }
}
