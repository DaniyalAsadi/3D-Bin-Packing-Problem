using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;

public interface ISubBinUpdatingAlgorithm
{
    List<SubBin> Execute(List<SubBin> subBins, Item item);
}