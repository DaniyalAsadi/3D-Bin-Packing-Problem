using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;

public interface ISubBinUpdatingAlgorithm
{
    List<SubBin> Execute(List<SubBin> subBins, PlacementResult? placement);
}