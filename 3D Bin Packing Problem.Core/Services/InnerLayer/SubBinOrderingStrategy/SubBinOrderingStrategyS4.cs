using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

public class SubBinOrderingStrategyS4 : ISubBinOrderingStrategy
{
    private const float Tolerance = AppConstants.Tolerance;
    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        var enumerable = subBins as SubBin[] ?? subBins.ToArray();
        if (!enumerable.Any())
            return [];

        // هر SubBin را به همراه Rule محاسبه می‌کنیم
        var subBinWithRule = enumerable.Select(sub => new
        {
            SubBin = sub,
            Rule = CalculateRule(sub, item)
        });

        // مرتب‌سازی براساس Rule و سپس tie-break X,Y,Z
        var sorted = subBinWithRule
            .OrderBy(s => s.Rule)
            .ThenBy(s => s.SubBin.Position.X)
            .ThenBy(s => s.SubBin.Position.Y)
            .ThenBy(s => s.SubBin.Position.Z)
            .Select(s => s.SubBin);

        return sorted;
    }

    private static int CalculateRule(SubBin sub, Item item)
    {
        // الگوریتم 5: تعیین Rule براساس اندازه‌ها
        if (Math.Abs(sub.Size.Length - item.Dimensions.Length) < Tolerance &&
            Math.Abs(sub.Size.Width - item.Dimensions.Width) < Tolerance &&
            Math.Abs(sub.Size.Height - item.Dimensions.Height) < Tolerance)
            return 1;
        if (TwoDimensionsMatch(sub, item))
            return 2;
        if (LwMatch(sub, item))
            return 3;
        if (HMatches(sub, item))
            return 4;
        if (LOrWMatches(sub, item))
            return 5;

        return int.MaxValue; // اگر هیچ Rule ای برقرار نبود
    }

    private static bool TwoDimensionsMatch(SubBin sub, Item item)
    {
        // چک کردن اینکه هر دو از سه بعد L,W,H برابر باشند
        var subDims = new[] { sub.Size.Length, sub.Size.Width, sub.Size.Height };
        var itemDims = new[] { item.Dimensions.Length, item.Dimensions.Width, item.Dimensions.Height };
        var matchCount = subDims.Count(d => itemDims.Contains(d));
        return matchCount == 2;
    }

    private static bool LwMatch(SubBin sub, Item item)
    {
        // چک کردن تطابق L و W دقیق
        return (Math.Abs(sub.Size.Length - item.Dimensions.Length) < Tolerance &&
                Math.Abs(sub.Size.Width - item.Dimensions.Width) < Tolerance) ||
               (Math.Abs(sub.Size.Length - item.Dimensions.Width) < Tolerance &&
                Math.Abs(sub.Size.Width - item.Dimensions.Length) < Tolerance);
    }

    private static bool HMatches(SubBin sub, Item item)
    {
        return Math.Abs(sub.Size.Height - item.Dimensions.Length) < Tolerance ||
               Math.Abs(sub.Size.Height - item.Dimensions.Width) < Tolerance ||
               Math.Abs(sub.Size.Height - item.Dimensions.Height) < Tolerance;
    }

    private static bool LOrWMatches(SubBin sub, Item item)
    {
        return Math.Abs(sub.Size.Length - item.Dimensions.Length) < Tolerance ||
               Math.Abs(sub.Size.Length - item.Dimensions.Width) < Tolerance ||
               Math.Abs(sub.Size.Length - item.Dimensions.Height) < Tolerance ||
               Math.Abs(sub.Size.Width - item.Dimensions.Length) < Tolerance ||
               Math.Abs(sub.Size.Width - item.Dimensions.Width) < Tolerance ||
               Math.Abs(sub.Size.Width - item.Dimensions.Height) < Tolerance;
    }
}