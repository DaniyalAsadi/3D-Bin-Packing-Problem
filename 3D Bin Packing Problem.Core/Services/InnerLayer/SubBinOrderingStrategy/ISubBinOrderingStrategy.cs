using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
public interface ISubBinOrderingStrategy
{
    IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item);
}