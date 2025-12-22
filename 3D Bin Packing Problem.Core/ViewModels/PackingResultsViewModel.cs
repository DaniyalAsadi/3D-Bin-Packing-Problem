using _3D_Bin_Packing_Problem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Aggregates the full packing outcome including packed items, remaining items, and utilized bin types.
/// </summary>
public class PackingResultsViewModel
{
    public List<PackedItemViewModel> PackedItems { get; set; } = new();
    public List<ItemViewModel> LeftItems { get; set; } = new();
    public List<BinInstance> UsedBinTypes { get; set; } = new();

    public int TotalPackedItems => PackedItems.Count;
    public int TotalLeftItems => LeftItems.Count;
    public int TotalUsedBins => UsedBinTypes.Count;
    public decimal TotalCost => UsedBinTypes.Sum(binInstance => binInstance.BinType.Cost);
    public float TotalPackedVolume => PackedItems.Sum(item => item.Volume);
    public double TotalBinVolume => UsedBinTypes.Sum(binInstance => binInstance.BinType.Volume);
    public double SpaceUtilization => TotalBinVolume > 0 ? TotalPackedVolume / TotalBinVolume : 0;

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();

        // Header
        sb.AppendLine("╔══════════════════════════════════════╗");
        sb.AppendLine("║          PACKING RESULTS             ║");
        sb.AppendLine("╚══════════════════════════════════════╝");

        // Summary Section
        sb.AppendLine("📊 SUMMARY");
        sb.AppendLine($"   • Packed Items: {TotalPackedItems}");
        sb.AppendLine($"   • Left Items: {TotalLeftItems}");
        sb.AppendLine($"   • Used Bins: {TotalUsedBins}");
        sb.AppendLine($"   • Total Cost: {TotalCost:F2}");
        sb.AppendLine($"   • Space Utilization: {SpaceUtilization:P2}");
        sb.AppendLine();

        // Packed Items Section
        if (PackedItems.Any())
        {
            sb.AppendLine("📦 PACKED ITEMS");
            foreach (PackedItemViewModel? item in PackedItems.Take(10)) // Show first 10 items
            {
                sb.AppendLine($"   ✓ {item}");
            }
            if (PackedItems.Count > 10)
            {
                sb.AppendLine($"   ... and {PackedItems.Count - 10} more items");
            }
            sb.AppendLine();
        }

        // Left Items Section
        if (LeftItems.Any())
        {
            sb.AppendLine("⚠️  LEFT ITEMS (UNPACKED)");
            foreach (var item in LeftItems.Take(5)) // Show first 5 left items
            {
                sb.AppendLine($"   ✗ {item}");
            }
            if (LeftItems.Count > 5)
            {
                sb.AppendLine($"   ... and {LeftItems.Count - 5} more items");
            }
            sb.AppendLine();
        }

        // Used Bins Section
        if (UsedBinTypes.Any())
        {
            sb.AppendLine("🗳️  USED BIN TYPES");
            foreach (var binInstance in UsedBinTypes)
            {
                sb.AppendLine($"   🗂️  {binInstance.BinType.Name ?? "Unnamed Bin"}");
                sb.AppendLine($"      InstanceId: {binInstance.ClonedInstance}");
                sb.AppendLine($"      Dimensions: {binInstance.BinType.InnerDimensions.Length} × {binInstance.BinType.InnerDimensions.Width} × {binInstance.BinType.InnerDimensions.Height}");
                sb.AppendLine($"      Volume: {binInstance.BinType.Volume} | Cost: {binInstance.BinType.Cost:F2}");
            }
        }

        // Footer
        sb.AppendLine();
        sb.AppendLine("╔══════════════════════════════════════╗");
        sb.AppendLine("║             END REPORT               ║");
        sb.AppendLine("╚══════════════════════════════════════╝");

        return sb.ToString();
    }
}
