using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins by their distance from the origin to prefer placements near the bin's corner.
/// </summary>
public class SubBinOrderingStrategyS5 : ISubBinOrderingStrategy
{
    private double ComputeCornerDistance(SubBin sb)
    {
        return Math.Sqrt(sb.Position.X * sb.Position.X + sb.Position.Y * sb.Position.Y + sb.Position.Z * sb.Position.Z);
    }

    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderBy(ComputeCornerDistance)
            .ThenByDescending(sb => sb.Volume)
            .ThenBy(sb => sb.Position.X)
            .ThenBy(sb => sb.Position.Y)
            .ThenBy(sb => sb.Position.Z)
            .ThenBy(_ => item.Id);
    }
}