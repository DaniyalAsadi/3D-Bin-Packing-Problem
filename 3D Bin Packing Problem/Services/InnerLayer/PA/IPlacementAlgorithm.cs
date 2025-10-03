using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.PA;

public interface IPlacementAlgorithm
{
    PackingResultsViewModel Execute(List<Item> items, List<BinType> bins);
}