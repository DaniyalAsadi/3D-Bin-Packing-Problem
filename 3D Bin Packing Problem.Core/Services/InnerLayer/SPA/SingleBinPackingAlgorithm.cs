using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;

/// <summary>
/// Executes the single-bin packing process by evaluating feasible placements and updating sub-bins accordingly.
/// </summary>
public class SingleBinPackingAlgorithm(
    IPlacementFeasibilityChecker feasibilityChecker,
    ISubBinUpdatingAlgorithm subBinUpdatingAlgorithm,
    SubBinOrderingStrategyFactory subBinOrderingStrategyFactory // 🔹 استراتژی مرتب‌سازی SubBin
) : ISingleBinPackingAlgorithm
{
    public PackingResultViewModel Execute(List<Item> items, BinInstance binInstance)
    {
        var subBinOrderingStrategy = subBinOrderingStrategyFactory.Create(SettingsManager.Current.SubBinOrdering);
        var itemList = items.ToList();
        var binType = binInstance.BinType;
        var subBinList = new List<SubBin> { binType };
        var leftItemList = new List<Item>();
        var packedItemList = new List<PlacementResult>();

        foreach (var item in itemList.ToList())
        {
            // 🔹 استفاده از ApplySpeedUpStrategy به جای شرط inline
            var validSubBins = ApplySpeedUpStrategy(subBinList, [item]);

            if (!validSubBins.Any())
            {
                leftItemList.Add(item);
                itemList.Remove(item);
                continue;
            }

            // 🔹 مرتب‌سازی SubBinها بر اساس استراتژی انتخابی (S1..S5)
            validSubBins = subBinOrderingStrategy.Apply(validSubBins, item).ToList();

            var placed = false;

            foreach (var validSubBin in validSubBins)
            {
                if (!feasibilityChecker.Execute(binType, item, validSubBin, out var placementResult)) continue;
                if (placementResult is null) throw new ArgumentNullException(nameof(placementResult));

                packedItemList.Add(placementResult);

                // 🔹 آپدیت SubBin باید بر اساس placementResult انجام شود، نه فقط item
                subBinList = subBinUpdatingAlgorithm.Execute(subBinList, placementResult);

                placed = true;
                break; // اولین SubBin معتبر انتخاب می‌شود
            }

            // 🔹 اگر در هیچ SubBin جا نشد → به لیست LeftItems اضافه می‌شود
            if (!placed)
                leftItemList.Add(item);

            itemList.Remove(item);
        }

        return new PackingResultViewModel
        {
            LeftItems = leftItemList.Select(x => new ItemViewModel
            {
                Id = x.Id,
                Height = x.Dimensions.Height,
                Length = x.Dimensions.Length,
                Width = x.Dimensions.Width,
            }).ToList(),

            PackedItems = packedItemList.Select(x => new PackedItemViewModel
            {
                ItemId = x.Item.Id,
                Item = x.Item,
                BinTypeId = x.BinType.Id,
                InstanceId = binInstance.ClonedInstance,
                Position = new Vector3(x.Position.X, x.Position.Y, x.Position.Z),
                Length = x.Orientation.X,
                Width = x.Orientation.Y,
                Height = x.Orientation.Z,
                SupportRatio = x.SupportRatio,
            }).ToList(),

            // 🔹 SubBinهای باقی‌مانده را هم برمی‌گردانیم (مطابق مقاله)
            RemainingSubBins = subBinList.Select(x => new SubBinViewModel()
            {
                Height = x.Height,
                Length = x.Length,
                Width = x.Width,
                X = x.X,
                Y = x.Y,
                Z = x.Z
            }).ToList()
        };
    }

    /// <summary>
    /// Speed-up strategy برای حذف SubBinهایی که هیچ آیتمی نمی‌تواند داخلشان قرار بگیرد
    /// </summary>
    private static List<SubBin> ApplySpeedUpStrategy(List<SubBin> subBins, List<Item> items)
    {
        if (items.Count == 0) return [];

        return subBins.Where(sb =>
            items.Any(item =>
                sb.Volume >= item.Volume &&
                sb.GetMinimumDimension() >= item.GetMinimumDimension()
            )
        ).ToList();
    }
}
