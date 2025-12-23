using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SC;

public class StabilityChecker : IStabilityChecker
{

    public bool IsStable(PackingResultsViewModel packingResult)
    {
        IEnumerable<PackedItemViewModel>? placedItems = packingResult.PackedItems.AsEnumerable(); // List<PackedItem>

        // گروه‌بندی آیتم‌ها بر اساس BinInstance
        var bins = placedItems.GroupBy(p => p.InstanceId);

        foreach (var bin in bins)
        {
            var itemsInBin = bin.ToList();

            // چک Fragile: فقط روی کف یا SupportRatio == 1
            foreach (var p in itemsInBin)
            {
                if (p.Item.IsFragile)
                {
                    if (p.Position.Z != 0 || p.SupportRatio > 1.0)
                        return false;
                }
            }

            // چک Stackable و MaxLoadOnTop
            // محاسبه وزن روی هر آیتم
            var loadOnItem = new Dictionary<Guid, decimal>();

            foreach (var p in itemsInBin)
                loadOnItem[p.Item.Id] = 0;

            foreach (var bottom in itemsInBin)
            {
                if (!bottom.Item.IsStackable) continue;

                decimal maxLoad = bottom.Item.MaxLoadOnTop ?? decimal.MaxValue;

                decimal load = 0;
                foreach (var top in itemsInBin)
                {
                    if (top.Item.Id == bottom.Item.Id) continue;

                    // چک هم‌پوشانی سطحی (برای ساپورت)
                    if (HasVerticalOverlap(bottom, top))
                    {
                        load += top.Item.Weight;
                    }
                }

                if (load > maxLoad)
                    return false;

                loadOnItem[bottom.Item.Id] = load;
            }
        }

        return true;
    }

    private static bool HasVerticalOverlap(PackedItemViewModel bottom, PackedItemViewModel top)
    {
        // top باید دقیقاً روی bottom باشد
        if (top.Position.Z < bottom.Position.Z + bottom.Height)
            return false;

        // بررسی هم‌پوشانی در محور X
        var overlapX =
            top.Position.X < bottom.Position.X + bottom.Length &&
            top.Position.X + top.Length > bottom.Position.X;

        if (!overlapX)
            return false;

        // بررسی هم‌پوشانی در محور Y
        var overlapY =
            top.Position.Y < bottom.Position.Y + bottom.Width &&
            top.Position.Y + top.Width > bottom.Position.Y;

        return overlapY;
    }

}