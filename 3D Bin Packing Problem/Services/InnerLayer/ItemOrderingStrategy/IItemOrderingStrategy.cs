using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.ItemOrderingStrategy;

public interface IItemOrderingStrategy
{
    IEnumerable<Item> Apply(IEnumerable<Item> items);
}