using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins using a fit rate metric to prioritize the most proportionally compatible spaces.
/// </summary>
public class SubBinOrderingStrategyS3 : ISubBinOrderingStrategy
{
    private double ComputeFitRate(Item item, SubBin sb)
    {
        var frLength = Math.Min((double)item.Length / sb.Length, (double)sb.Length / item.Length);
        var frWidth = Math.Min((double)item.Width / sb.Width, (double)sb.Width / item.Width);
        var frHeight = Math.Min((double)item.Height / sb.Height, (double)sb.Height / item.Height);

        return frLength * frWidth * frHeight;
    }

    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderByDescending(sb => ComputeFitRate(item, sb));
    }
}
