public class BenchmarkSuite
{
    public List<ClassBenchmark> Classes { get; set; } = new List<ClassBenchmark>();

    public void PrintSummary()
    {
        Console.WriteLine("=== Complete Benchmark Suite Summary ===");
        Console.WriteLine($"Total Classes: {Classes.Count}");
        Console.WriteLine($"Total Instances: {Classes.Sum(c => c.Instances.Count)}");
        Console.WriteLine();

        foreach (var classBenchmark in Classes)
        {
            classBenchmark.PrintSummary();
            Console.WriteLine();
        }
    }

    public BenchmarkInstance GetInstance(string instanceName)
    {
        return Classes
            .SelectMany(c => c.Instances)
            .FirstOrDefault(i => i.InstanceName == instanceName);
    }

    public List<BenchmarkInstance> GetInstancesByClass(ItemClass itemClass)
    {
        return Classes
            .Where(c => c.ItemClass == itemClass)
            .SelectMany(c => c.Instances)
            .ToList();
    }
}