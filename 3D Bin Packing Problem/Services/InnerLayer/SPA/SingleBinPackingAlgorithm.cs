using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;

internal class SingleBinPackingAlgorithm(
    IPlacementFeasibilityChecker feasibilityChecker,
    ISubBinUpdatingAlgorithm subBinUpdatingAlgorithm)
    : ISingleBinPackingAlgorithm
{


    public PackingResultViewModel Execute(List<Item> items, BinType binType)
    {
        var itemList = items.ToList();
        List<SubBin> subBinList = [];
        List<Item> leftItemList = [];
        List<PlacementResult> packedItemList = [];
        subBinList.Add(binType);
        foreach (var item in itemList.ToList())
        {
            // Apply the speedup strategies and get valid sub-bins
            var validSubBins = subBinList
                .Where(sb => sb.Volume >= item.Volume) // شرط ۱
                .Where(sb => sb.GetMinimumDimension() >= item.GetMinimumDimension()) // شرط ۲
                .ToList();
            foreach (var validSubBin in validSubBins)
            {
                if (feasibilityChecker.Execute(item, validSubBin, out var placementResult))
                {
                    ArgumentNullException.ThrowIfNull(placementResult);
                    packedItemList.Add(placementResult);
                    subBinUpdatingAlgorithm.Execute(subBinList, item);
                    break;
                }
                else
                {
                    if (subBinList.IndexOf(validSubBin) == subBinList.IndexOf(subBinList.Last()))
                    {
                        leftItemList.Add(item);
                    }
                }
            }

            itemList.Remove(item);
        }

        return new PackingResultViewModel()
        {
            LeftItems = leftItemList.Select(x => new ItemViewModel()
            {
                Id = x.Id,
                Height = x.Height,
                Length = x.Length,
                Width = x.Width,
            }).ToList(),
            PackedItems = packedItemList.Select(x => new PackedItemViewModel()
            {
                ItemId = x.Item.Id,
                X = (int)x.Position.X,
                Y = (int)x.Position.Y,
                Z = (int)x.Position.Z,
                Length = (int)x.Orientation.X,
                Width = (int)x.Orientation.Y,
                Height = (int)x.Orientation.Z,
                SupportRatio = x.SupportRatio,

            }).ToList(),
            RemainingSubBins = null,
        };
    }

    private List<SubBin> ApplySpeedUpStrategy(List<SubBin> subBins, List<Item> items)
    {
        if (items.Count == 0)
            return [];

        var validSubBins = new List<SubBin>();

        foreach (var sb in subBins)
        {
            var canHoldAnyItem = items.Any(item =>
                sb.Volume >= item.Volume &&
                Math.Min(sb.Length, Math.Min(sb.Width, sb.Height)) >=
                Math.Min(item.Length, Math.Min(item.Width, item.Height))
            );

            if (canHoldAnyItem)
                validSubBins.Add(sb);
        }

        return validSubBins;
    }

}