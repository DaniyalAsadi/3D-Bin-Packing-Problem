using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.ViewModels;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;

public interface IPlacementFeasibilityChecker
{
    bool Execute(BinType binType, Item item, SubBin subBin, out PlacementResult? result);
}