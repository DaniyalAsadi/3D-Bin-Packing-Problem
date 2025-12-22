using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;

public interface IPlacementAlgorithm
{
    PackingResultsViewModel Execute(List<Item> items, List<BinType> bins);
}