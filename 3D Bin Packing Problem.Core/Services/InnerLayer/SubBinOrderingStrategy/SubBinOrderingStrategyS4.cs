using _3D_Bin_Packing_Problem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

public class SubBinOrderingStrategyS4 : ISubBinOrderingStrategy
{
    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        var array = subBins as SubBin[] ?? subBins.ToArray();
        if (array.Length == 0)
            return Array.Empty<SubBin>();

        var subBinWithRule = array.Select(sub => new
        {
            SubBin = sub,
            Rule = CalculateRule(sub, item)
        });

        return subBinWithRule
            .OrderBy(s => s.Rule)
            .ThenBy(s => s.SubBin.Position.X)
            .ThenBy(s => s.SubBin.Position.Y)
            .ThenBy(s => s.SubBin.Position.Z)
            .Select(s => s.SubBin);
    }

    private static int CalculateRule(SubBin sub, Item item)
    {
        // Rule 1: exact match of all 3 dimensions
        if (sub.Size.Length == item.Dimensions.Length &&
            sub.Size.Width == item.Dimensions.Width &&
            sub.Size.Height == item.Dimensions.Height)
            return 1;

        // Rule 2: exactly two matching dimensions
        if (TwoDimensionsMatch(sub, item))
            return 2;

        // Rule 3: L/W match (in either orientation)
        if (LwMatch(sub, item))
            return 3;

        // Rule 4: height matches any dimension
        if (HMatches(sub, item))
            return 4;

        // Rule 5: L or W matches any dimension
        if (LOrWMatches(sub, item))
            return 5;

        return int.MaxValue;
    }

    private static bool TwoDimensionsMatch(SubBin sub, Item item)
    {
        var subDims = new[] { sub.Size.Length, sub.Size.Width, sub.Size.Height };
        var itemDims = new[] { item.Dimensions.Length, item.Dimensions.Width, item.Dimensions.Height };

        int matchCount = subDims.Count(d => itemDims.Contains(d));
        return matchCount == 2;
    }

    private static bool LwMatch(SubBin sub, Item item)
    {
        return
            (sub.Size.Length == item.Dimensions.Length &&
             sub.Size.Width == item.Dimensions.Width) ||

            (sub.Size.Length == item.Dimensions.Width &&
             sub.Size.Width == item.Dimensions.Length);
    }

    private static bool HMatches(SubBin sub, Item item)
    {
        return
            sub.Size.Height == item.Dimensions.Length ||
            sub.Size.Height == item.Dimensions.Width ||
            sub.Size.Height == item.Dimensions.Height;
    }

    private static bool LOrWMatches(SubBin sub, Item item)
    {
        return
            sub.Size.Length == item.Dimensions.Length ||
            sub.Size.Length == item.Dimensions.Width ||
            sub.Size.Length == item.Dimensions.Height ||
            sub.Size.Width == item.Dimensions.Length ||
            sub.Size.Width == item.Dimensions.Width ||
            sub.Size.Width == item.Dimensions.Height;
    }
}
