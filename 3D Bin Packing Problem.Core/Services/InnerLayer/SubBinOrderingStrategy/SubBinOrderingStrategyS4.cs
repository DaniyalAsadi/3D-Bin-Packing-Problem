using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

public class SubBinOrderingStrategyS4 : ISubBinOrderingStrategy
{
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
            .ThenBy(s => s.SubBin.X)
            .ThenBy(s => s.SubBin.Y)
            .ThenBy(s => s.SubBin.Z)
            .Select(s => s.SubBin);

        return sorted;
    }

    private int CalculateRule(SubBin sub, Item item)
    {
        // الگوریتم 5: تعیین Rule براساس اندازه‌ها
        if (sub.Length == item.Length && sub.Width == item.Width && sub.Height == item.Height)
            return 1;
        if (TwoDimensionsMatch(sub, item))
            return 2;
        if (LWMatch(sub, item))
            return 3;
        if (HMatches(sub, item))
            return 4;
        if (LOrWMatches(sub, item))
            return 5;

        return int.MaxValue; // اگر هیچ Rule ای برقرار نبود
    }

    private bool TwoDimensionsMatch(SubBin sub, Item item)
    {
        // چک کردن اینکه هر دو از سه بعد L,W,H برابر باشند
        var subDims = new[] { sub.Length, sub.Width, sub.Height };
        var itemDims = new[] { item.Length, item.Width, item.Height };
        int matchCount = subDims.Count(d => itemDims.Contains(d));
        return matchCount == 2;
    }

    private bool LWMatch(SubBin sub, Item item)
    {
        // چک کردن تطابق L و W دقیق
        return (sub.Length == item.Length && sub.Width == item.Width) || (sub.Length == item.Width && sub.Width == item.Length);
    }

    private bool HMatches(SubBin sub, Item item)
    {
        return sub.Height == item.Length || sub.Height == item.Width || sub.Height == item.Height;
    }

    private bool LOrWMatches(SubBin sub, Item item)
    {
        return sub.Length == item.Length || sub.Length == item.Width || sub.Length == item.Height ||
               sub.Width == item.Length || sub.Width == item.Width || sub.Width == item.Height;
    }
}