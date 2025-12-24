using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;

/// <summary>
/// Implements the placement algorithm that orchestrates item ordering, bin selection, and single-bin packing.
/// </summary>
public class PlacementAlgorithm(
    ItemOrderingStrategyFactory itemOrderingStrategyFactory,
    SubBinSelectionStrategyFactory subBinSelectionStrategyFactory,
    ISingleBinPackingAlgorithm singleBinPackingAlgorithm) : IPlacementAlgorithm
{
    public PackingResultsViewModel Execute(List<Item> items, List<BinType> bins)
    {
        var itemOrderingStrategy = itemOrderingStrategyFactory.Create(SettingsManager.Current.ItemOrdering, bins);
        var subBinSelectionStrategy = subBinSelectionStrategyFactory.Create(SettingsManager.Current.SubBinSelection);
        var sortedItems = itemOrderingStrategy.Apply(items).ToList();
        var leftItemList = sortedItems;
        var packingResultsViewModel = new PackingResultsViewModel();

        while (leftItemList.Count != 0)
        {
            // انتخاب Bin بر اساس استراتژی
            var binType = subBinSelectionStrategy.Execute(bins, leftItemList);

            if (binType is null)
            {
                // اگر هیچ Bin مناسبی نبود → آیتم‌های باقیمانده به عنوان Unpacked ذخیره شوند
                packingResultsViewModel.LeftItems.AddRange(
                    leftItemList.Select(i => new ItemViewModel { Id = i.Id })
                );
                break;
            }

            var binInstance = new BinInstance(binType);
            // اجرای الگوریتم Single Bin Packing
            var packingResultViewModel = singleBinPackingAlgorithm.Execute(leftItemList, binInstance);

            // اضافه کردن آیتم‌های بسته‌بندی شده
            packingResultsViewModel.PackedItems.AddRange(packingResultViewModel.PackedItems);
            packingResultsViewModel.UsedBinTypes.Add(binInstance);
            // به‌روزرسانی لیست آیتم‌های باقی‌مانده
            leftItemList = leftItemList
                .Where(item => packingResultViewModel.LeftItems.Any(ivm => ivm.Id == item.Id))
                .ToList();
        }

        return packingResultsViewModel;
    }
}