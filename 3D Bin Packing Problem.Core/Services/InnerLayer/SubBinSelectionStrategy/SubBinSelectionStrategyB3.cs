using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

/// <summary>
/// Chooses the lowest-cost bin that satisfies the size requirements of the remaining items.
/// </summary>
public class SubBinSelectionStrategyB3 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        var feasibleBins = FilterFeasibleBins(binTypes, items);

        return feasibleBins
            .OrderBy(bt => bt.Cost)
            .FirstOrDefault();
    }

    private IEnumerable<BinType> FilterFeasibleBins(IEnumerable<BinType> binTypes, List<Item> items)
    {
        var maxLength = items.Max(i => i.Dimensions.Length);
        var maxWidth = items.Max(i => i.Dimensions.Width);
        var maxHeight = items.Max(i => i.Dimensions.Height);

        return binTypes.Where(bt =>
            bt.InnerDimensions.Length >= maxLength &&
            bt.InnerDimensions.Width >= maxWidth &&
            bt.InnerDimensions.Height >= maxHeight);
    }
}
