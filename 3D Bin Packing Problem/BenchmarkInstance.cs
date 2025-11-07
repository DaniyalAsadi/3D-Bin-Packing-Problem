using _3D_Bin_Packing_Problem.Core.Model;

public class BenchmarkInstance
{
    public string InstanceName { get; set; } = string.Empty;
    public ItemClass ItemClass { get; set; }
    public int ItemCount { get; set; }
    public int InstanceNumber { get; set; }
    public List<Item> Items { get; set; } = [];
    public List<BinType> Bins { get; set; } = [];

    public void PrintSummary()
    {
        Console.WriteLine($"  {InstanceName}: {Items.Count} items, {Bins.Count} bins");
        if (Items.Count <= 0) return;
        var firstItem = Items[0];
        Console.WriteLine($"    First item: {firstItem}, Volume: {firstItem.Volume}");
    }
}