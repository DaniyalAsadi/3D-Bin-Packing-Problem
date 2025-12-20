using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins by prioritizing those positioned furthest along the X-axis, then lowest in Y and Z.
/// </summary>
public class SubBinOrderingStrategyS1 : ISubBinOrderingStrategy
{
    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderByDescending(sb => sb.Position.X)   // X غیر افزایشی
            .ThenBy(sb => sb.Position.Y)              // Y افزایشی
            .ThenBy(sb => sb.Position.Z);             // Z افزایشی
    }
}
