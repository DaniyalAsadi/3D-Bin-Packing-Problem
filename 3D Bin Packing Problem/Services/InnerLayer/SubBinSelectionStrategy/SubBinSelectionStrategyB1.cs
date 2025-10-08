using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

/// <summary>
/// Chooses the most cost-efficient bin that can accommodate at least one remaining item.
/// </summary>
public class SubBinSelectionStrategyB1 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        var feasibleBins = FilterFeasibleBins(binTypes, items);

        return feasibleBins
            .OrderBy(bt => bt.Cost / (double)bt.Volume)
            .FirstOrDefault();
    }

    private IEnumerable<BinType> FilterFeasibleBins(IEnumerable<BinType> binTypes, List<Item> items)
    {
        // فقط Binهایی که حداقل یکی از آیتم‌ها را می‌توانند accommodate کنند
        return binTypes.Where(bt =>
            items.Any(i =>
                i.Length <= bt.Length &&
                i.Width <= bt.Width &&
                i.Height <= bt.Height));
    }
}
