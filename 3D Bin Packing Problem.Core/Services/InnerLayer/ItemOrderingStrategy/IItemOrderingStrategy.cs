using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;

public interface IItemOrderingStrategy
{
    IEnumerable<Item> Apply(IEnumerable<Item> items);
}