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
        float maxLength = items.Max(i => i.Dimensions.Length);
        float maxWidth = items.Max(i => i.Dimensions.Width);
        float maxHeight = items.Max(i => i.Dimensions.Height);

        return binTypes.Where(bt =>
            bt.InnerDimensions.Length >= maxLength &&
            bt.InnerDimensions.Width >= maxWidth &&
            bt.InnerDimensions.Height >= maxHeight);
    }
}
