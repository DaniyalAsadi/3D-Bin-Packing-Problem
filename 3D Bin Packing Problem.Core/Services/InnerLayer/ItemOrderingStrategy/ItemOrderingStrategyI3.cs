using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;

/// <summary>
/// Orders items by bin type availability and cost to better align items with economical bin choices.
/// </summary>
public class ItemOrderingStrategyI3(IEnumerable<BinType> binTypes) : IItemOrderingStrategy
{
    private int ComputeBtn(Item item)
    {
        return binTypes.Count(bt =>
            item.Dimensions.Length <= bt.InnerDimensions.Length &&
            item.Dimensions.Width <= bt.InnerDimensions.Width &&
            item.Dimensions.Height <= bt.InnerDimensions.Height);
    }

    private decimal ComputeMinCost(Item item)
    {
        return binTypes
            .Where(bt =>
                item.Dimensions.Length <= bt.InnerDimensions.Length &&
                item.Dimensions.Width <= bt.InnerDimensions.Width &&
                item.Dimensions.Height <= bt.InnerDimensions.Height)
            .Select(bt => bt.Cost)
            .Min();
    }

    public IEnumerable<Item> Apply(IEnumerable<Item> items)
    {
        return items
            .OrderBy(ComputeBtn)
            .ThenBy(ComputeMinCost)
            .ThenByDescending(i => i.Volume)
            .ThenByDescending(i => i.Dimensions.Length)
            .ThenByDescending(i => i.Dimensions.Width)
            .ThenByDescending(i => i.Dimensions.Height)
            .ThenBy(i => i.Id);
    }
}
