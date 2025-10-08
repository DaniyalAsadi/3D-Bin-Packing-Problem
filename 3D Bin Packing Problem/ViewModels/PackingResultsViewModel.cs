using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.ViewModels;

/// <summary>
/// Aggregates the full packing outcome including packed items, remaining items, and utilized bin types.
/// </summary>
public class PackingResultsViewModel
{

    public List<PackedItemViewModel> PackedItems { get; set; } = [];

    public List<ItemViewModel> LeftItems { get; set; } = [];

    public List<BinType> UsedBinTypes { get; set; } = [];
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("=== Packing Results ===");
        sb.AppendLine($"Packed Items ({PackedItems.Count}):");

        foreach (var item in PackedItems)
            sb.AppendLine($"  - {item}");

        sb.AppendLine($"Left Items ({LeftItems.Count}):");
        foreach (var item in LeftItems)
            sb.AppendLine($"  - {item}");

        sb.AppendLine($"Used Bin Types ({UsedBinTypes.Count}):");
        foreach (var bin in UsedBinTypes)
            sb.AppendLine($"  - {bin}");

        return sb.ToString();
    }
}
