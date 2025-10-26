using _3D_Bin_Packing_Problem.Core.Model;
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
    IItemOrderingStrategy itemOrderingStrategy,
    ISubBinSelectionStrategy subBinSelectionStrategy,
    ISingleBinPackingAlgorithm singleBinPackingAlgorithm) : IPlacementAlgorithm
{
    public PackingResultsViewModel Execute(List<Item> items, List<BinType> bins)
    {
        items = itemOrderingStrategy.Apply(items).ToList();
        var leftItemList = items;
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

            // اجرای الگوریتم Single Bin Packing
            var packingResultViewModel = singleBinPackingAlgorithm.Execute(leftItemList, binType);

            // اضافه کردن آیتم‌های بسته‌بندی شده
            packingResultsViewModel.PackedItems.AddRange(packingResultViewModel.PackedItems);
            packingResultsViewModel.UsedBinTypes.Add(binType);
            // به‌روزرسانی لیست آیتم‌های باقی‌مانده
            leftItemList = leftItemList
                .Where(item => packingResultViewModel.LeftItems.Any(ivm => ivm.Id == item.Id))
                .ToList();
        }

        return packingResultsViewModel;
    }
}
