using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.PA;

public interface IPlacementAlgorithm
{
    PackingResultsViewModel Execute(List<Item> items, List<BinType> bins);
}
internal class PlacementAlgorithm(
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
            var binType = subBinSelectionStrategy.Execute(bins, items);
            ArgumentNullException.ThrowIfNull(binType);
            var packingResultViewModel = singleBinPackingAlgorithm.Execute(items, binType);
            packingResultsViewModel.PackedItems.AddRange(packingResultViewModel.PackedItems);

            leftItemList = items.Where(item => packingResultViewModel.LeftItems.Select(ivm => ivm.Id).Contains(item.Id)).ToList();

        }

        return packingResultsViewModel;

    }
}

