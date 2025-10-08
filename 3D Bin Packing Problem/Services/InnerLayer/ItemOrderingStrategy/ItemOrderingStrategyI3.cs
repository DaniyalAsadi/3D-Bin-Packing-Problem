using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.ItemOrderingStrategy;

/// <summary>
/// Orders items by bin type availability and cost to better align items with economical bin choices.
/// </summary>
public class ItemOrderingStrategyI3(IEnumerable<BinType> binTypes) : IItemOrderingStrategy
{
    private int ComputeBTN(Item item)
    {
        return binTypes.Count(bt =>
            item.Length <= bt.Length &&
            item.Width <= bt.Width &&
            item.Height <= bt.Height);
    }

    private double ComputeMinCost(Item item)
    {
        return binTypes
            .Where(bt =>
                item.Length <= bt.Length &&
                item.Width <= bt.Width &&
                item.Height <= bt.Height)
            .Select(bt => bt.Cost)
            .DefaultIfEmpty(double.MaxValue)
            .Min();
    }

    public IEnumerable<Item> Apply(IEnumerable<Item> items)
    {
        return items
            .OrderBy(ComputeBTN)
            .ThenBy(ComputeMinCost)
            .ThenByDescending(i => i.Volume)
            .ThenByDescending(i => i.Length)
            .ThenByDescending(i => i.Width)
            .ThenByDescending(i => i.Height)
            .ThenBy(i => i.Id);
    }
}
