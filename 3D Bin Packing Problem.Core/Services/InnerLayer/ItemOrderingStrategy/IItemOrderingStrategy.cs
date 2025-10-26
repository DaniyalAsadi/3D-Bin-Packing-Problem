using System.Collections.Generic;
using _3D_Bin_Packing_Problem.Core.Model;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;

public interface IItemOrderingStrategy
{
    IEnumerable<Item> Apply(IEnumerable<Item> items);
}