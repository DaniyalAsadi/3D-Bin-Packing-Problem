using _3D_Bin_Packing_Problem.Core.Model;

public class BenchmarkInstance
{
    public string InstanceName { get; set; }
    public ItemClass ItemClass { get; set; }
    public int ItemCount { get; set; }
    public int InstanceNumber { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public List<BinType> Bins { get; set; } = new List<BinType>();

    public void PrintSummary()
    {
        Console.WriteLine($"  {InstanceName}: {Items.Count} items, {Bins.Count} bins");
        if (Items.Count > 0)
        {
            var firstItem = Items[0];
            Console.WriteLine($"    First item: {firstItem}, Volume: {firstItem.Volume}");
        }
    }
}