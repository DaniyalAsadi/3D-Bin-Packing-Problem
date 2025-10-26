using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

/// <summary>
/// Selects the smallest volume bin capable of containing the largest remaining dimensions.
/// </summary>
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
