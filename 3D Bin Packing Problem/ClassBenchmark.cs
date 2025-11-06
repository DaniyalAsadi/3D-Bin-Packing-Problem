public class ClassBenchmark
{
    public ItemClass ItemClass { get; set; }
    public ClassInfo ClassInfo { get; set; }
    public List<BenchmarkInstance> Instances { get; set; } = new List<BenchmarkInstance>();

    public void PrintSummary()
    {
        Console.WriteLine($"Class {(int)ItemClass + 1}:");
        Console.WriteLine($"  Bin Size: {ClassInfo.BinLength}×{ClassInfo.BinWidth}×{ClassInfo.BinHeight}");
        Console.WriteLine($"  Item Ranges: L[{ClassInfo.LengthRange.Item1}-{ClassInfo.LengthRange.Item2}], " +
                          $"W[{ClassInfo.WidthRange.Item1}-{ClassInfo.WidthRange.Item2}], " +
                          $"H[{ClassInfo.HeightRange.Item1}-{ClassInfo.HeightRange.Item2}]");
        Console.WriteLine($"  Instances: {Instances.Count}");

        foreach (var instance in Instances.Take(2)) // نمایش 2 نمونه اول
        {
            instance.PrintSummary();
        }
        if (Instances.Count > 2)
        {
            Console.WriteLine($"  ... and {Instances.Count - 2} more instances");
        }
    }
}