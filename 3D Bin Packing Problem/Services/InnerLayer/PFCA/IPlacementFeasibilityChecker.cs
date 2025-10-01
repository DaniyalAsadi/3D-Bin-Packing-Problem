using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;

public interface IPlacementFeasibilityChecker
{
    bool Execute(Item item, SubBin subBin, out PlacementResult? result);
}