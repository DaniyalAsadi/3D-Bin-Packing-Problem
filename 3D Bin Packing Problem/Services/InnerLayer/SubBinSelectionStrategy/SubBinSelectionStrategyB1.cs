using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

public class SubBinSelectionStrategyB1 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        return binTypes
            .OrderBy(bt => bt.Cost / bt.Volume)
            .ThenByDescending(bt => bt.Volume)
            .ThenByDescending(bt => bt.Length)
            .ThenByDescending(bt => bt.Width)
            .ThenByDescending(bt => bt.Height)
            .FirstOrDefault();
    }
}