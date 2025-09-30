using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderingStrategy;
public interface ISubBinOrderingStrategy
{
    IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item);
}