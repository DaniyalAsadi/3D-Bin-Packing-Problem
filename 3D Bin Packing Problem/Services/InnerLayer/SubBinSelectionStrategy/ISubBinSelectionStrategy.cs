using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

public interface ISubBinSelectionStrategy
{
    BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items);
}