using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.ItemOrderingStrategy;

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