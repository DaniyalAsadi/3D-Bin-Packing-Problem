using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

public class SubBinSelectionStrategyB3 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        if (items.Count == 0)
            return null;

        var firstItem = items.First();

        var fitBins = binTypes
            .Where(bt =>
                firstItem.Length <= bt.Length &&
                firstItem.Width <= bt.Width &&
                firstItem.Height <= bt.Height)
            .ToList();

        return fitBins
            .OrderBy(bt => bt.Cost / bt.Volume)
            .ThenByDescending(bt => bt.Volume)
            .ThenByDescending(bt => bt.Length)
            .ThenByDescending(bt => bt.Width)
            .ThenByDescending(bt => bt.Height)
            .FirstOrDefault();
    }
}