using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;

public interface ISingleBinPackingAlgorithm
{
    PackingResultViewModel Execute(List<Item> items, BinType binType);

}