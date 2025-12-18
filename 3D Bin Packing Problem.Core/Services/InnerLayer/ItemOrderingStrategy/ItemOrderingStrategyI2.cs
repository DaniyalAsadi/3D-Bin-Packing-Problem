using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;

/// <summary>
/// Orders items by their maximum face area to favor pieces with larger placement surfaces.
/// </summary>
public class ItemOrderingStrategyI2 : IItemOrderingStrategy
{
    public IEnumerable<Item> Apply(IEnumerable<Item> items)
    {
        return items
            .OrderByDescending(GetMaxArea)
            .ThenByDescending(i => i.Dimensions.Length)
            .ThenByDescending(i => i.Dimensions.Width)
            .ThenByDescending(i => i.Dimensions.Height)
            .ThenBy(i => i.Id);
    }
    private static float GetMaxArea(Item item)
    {
        var area1 = item.Dimensions.Length * item.Dimensions.Width;
        var area2 = item.Dimensions.Length * item.Dimensions.Height;
        var area3 = item.Dimensions.Width * item.Dimensions.Height;
        return Math.Max(area1, Math.Max(area2, area3));
    }

}
