using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

public class SubBinSelectionStrategyB2 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        var feasibleBins = FilterFeasibleBins(binTypes, items);

        return feasibleBins
            .OrderBy(bt => bt.Volume)
            .FirstOrDefault();
    }

    private IEnumerable<BinType> FilterFeasibleBins(IEnumerable<BinType> binTypes, List<Item> items)
    {
        int maxLength = items.Max(i => i.Length);
        int maxWidth = items.Max(i => i.Width);
        int maxHeight = items.Max(i => i.Height);

        return binTypes.Where(bt =>
            bt.Length >= maxLength &&
            bt.Width >= maxWidth &&
            bt.Height >= maxHeight);
    }
}