using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

public interface ISubBinSelectionStrategy
{
    BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items);
}