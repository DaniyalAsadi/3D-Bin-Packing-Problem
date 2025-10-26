using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;

/// <summary>
/// Orders items by descending volume and dimensions to prioritize larger items first.
/// </summary>
public class ItemOrderingStrategyI1 : IItemOrderingStrategy
{
    public IEnumerable<Item> Apply(IEnumerable<Item> items)
    {
        return items
            .OrderByDescending(i => i.Volume)
            .ThenByDescending(i => i.Length)
            .ThenByDescending(i => i.Width)
            .ThenByDescending(i => i.Height)
            .ThenBy(i => i.Id); // صعودی چون یکتا است
    }
}
