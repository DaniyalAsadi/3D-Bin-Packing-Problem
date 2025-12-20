using _3D_Bin_Packing_Problem.Core.ViewModels;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SC;
public interface IStabilityChecker
{
    bool IsStable(PackingResultsViewModel packingResult);
}