using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;

public interface ISingleBinPackingAlgorithm
{
    PackingResultViewModel Execute(List<Item> items, BinInstance binInstance);

}