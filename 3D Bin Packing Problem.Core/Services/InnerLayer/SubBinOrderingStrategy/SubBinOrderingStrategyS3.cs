using _3D_Bin_Packing_Problem.Core.Models;
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
        var frLength = Math.Min((double)item.Dimensions.Length / sb.Size.Length, (double)sb.Size.Length / item.Dimensions.Length);
        var frWidth = Math.Min((double)item.Dimensions.Width / sb.Size.Width, (double)sb.Size.Width / item.Dimensions.Width);
        var frHeight = Math.Min((double)item.Dimensions.Height / sb.Size.Height, (double)sb.Size.Height / item.Dimensions.Height);

        return frLength * frWidth * frHeight;
    }

    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderByDescending(sb => ComputeFitRate(item, sb));
    }
}
