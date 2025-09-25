using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderStrategy;
public interface ISubBinOrderingStrategy
{
    List<SubBin> Execute(List<SubBin> subBins, Item item);
}
