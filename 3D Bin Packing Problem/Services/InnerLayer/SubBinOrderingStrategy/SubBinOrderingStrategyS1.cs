using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins by prioritizing those positioned furthest along the X-axis, then lowest in Y and Z.
/// </summary>
public class SubBinOrderingStrategyS1 : ISubBinOrderingStrategy
{
    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderByDescending(sb => sb.X)   // X غیر افزایشی
            .ThenBy(sb => sb.Y)              // Y افزایشی
            .ThenBy(sb => sb.Z);             // Z افزایشی
    }
}
