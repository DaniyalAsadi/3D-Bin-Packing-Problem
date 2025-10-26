using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

/// <summary>
/// Chooses the most cost-efficient bin that can accommodate at least one remaining item.
/// </summary>
public class SubBinSelectionStrategyB1 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        // 1. مرتب‌سازی بر اساس معیارهای B1
        var orderedBins = binTypes
            .OrderByDescending(bt => bt.Cost / (double)bt.Volume)   // نزولی Cost/Volume
            .ThenByDescending(bt => bt.Volume)                      // نزولی حجم
            .ThenByDescending(bt => bt.Length)                      // نزولی طول
            .ThenByDescending(bt => bt.Width)                       // نزولی عرض
            .ThenByDescending(bt => bt.Height)                      // نزولی ارتفاع
            .ToList();

        // 2. فقط Binهایی که حداقل یکی از آیتم‌ها در آن‌ها جا می‌شوند
        var feasibleBins = orderedBins
            .Where(bt => items.Any(i => i.Length <= bt.Length &&
                                        i.Width <= bt.Width &&
                                        i.Height <= bt.Height))
            .ToList();

        // 3. Bin اول را برمی‌گردانیم یا null اگر هیچ Bin feasible وجود نداشت
        return feasibleBins.FirstOrDefault();
    }
}


