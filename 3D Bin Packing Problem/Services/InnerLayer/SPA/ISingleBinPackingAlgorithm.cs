using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;

public interface ISingleBinPackingAlgorithm
{
    PackingResultViewModel Execute(List<Item> items, Bin bin);

}

internal class SingleBinPackingAlgorithm(
    IPlacementFeasibilityChecker feasibilityChecker,
    ISubBinUpdatingAlgorithm subBinUpdatingAlgorithm)
    : ISingleBinPackingAlgorithm
{

    public PackingResultViewModel Execute(List<Item> items, Bin bin)
    {
        List<Item> itemList = items.ToList();
        List<SubBin> subBinList = [];
        List<Item> leftItemList = [];
        List<PlacementResult> packedItemList = [];
        List<SubBin> validSubBins = new List<SubBin>();
        subBinList.Add(bin);
        foreach (var item in itemList.ToList())
        {
            validSubBins = ApplySpeedUpStrategy();
            foreach (var validSubBin in validSubBins)
            {
                if (feasibilityChecker.Execute(item, validSubBin, out PlacementResult? placementResult))
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
            bool canHoldAnyItem = items.Any(item =>
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
