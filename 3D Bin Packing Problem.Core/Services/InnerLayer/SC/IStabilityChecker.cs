using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SC;
public interface IStabilityChecker
{
    bool IsStable(PackingResultsViewModel packingResult);
}
public class StabilityChecker : IStabilityChecker
{
    private const float Eps = AppConstants.Tolerance;

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
                    if (p.Position.Z > Eps || Math.Abs(p.SupportRatio - 1.0) > Eps)
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
        // top دقیقاً روی bottom
        return top.Position.Z >= bottom.Position.Z + bottom.Height - Eps &&
               top.Position.X < bottom.Position.X + bottom.Length + Eps &&
               top.Position.X + top.Length > bottom.Position.X - Eps &&
               top.Position.Y < bottom.Position.Y + bottom.Width + Eps &&
               top.Position.Y + top.Width > bottom.Position.Y - Eps;
    }
}