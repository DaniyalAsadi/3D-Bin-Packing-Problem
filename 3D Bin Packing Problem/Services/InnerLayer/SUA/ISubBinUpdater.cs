using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;

public interface ISubBinUpdatingAlgorithm
{
    List<SubBin> Execute(List<SubBin> subBins, PlacementResult? placement);
}