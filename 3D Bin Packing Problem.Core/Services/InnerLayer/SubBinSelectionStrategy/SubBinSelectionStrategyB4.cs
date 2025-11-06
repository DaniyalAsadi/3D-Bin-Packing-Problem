using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

/// <summary>
/// Selects the bin with the minimal surface area among those capable of fitting the remaining items.
/// </summary>
public class SubBinSelectionStrategyB4 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        var feasibleBins = FilterFeasibleBins(binTypes, items);

        return feasibleBins
            .OrderBy(bt => 2 * (bt.Length * bt.Width + bt.Length * bt.Height + bt.Width * bt.Height))
            .FirstOrDefault();
    }

    private IEnumerable<BinType> FilterFeasibleBins(IEnumerable<BinType> binTypes, List<Item> items)
    {
        var maxLength = items.Max(i => i.Length);
        var maxWidth = items.Max(i => i.Width);
        var maxHeight = items.Max(i => i.Height);

        return binTypes.Where(bt =>
            bt.Length >= maxLength &&
            bt.Width >= maxWidth &&
            bt.Height >= maxHeight);
    }
}
