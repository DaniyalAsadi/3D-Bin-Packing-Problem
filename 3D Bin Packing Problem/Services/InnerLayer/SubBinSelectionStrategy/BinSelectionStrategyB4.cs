using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

public class BinSelectionStrategyB4 : ISubBinSelectionStrategy
{
    public BinType? Execute(IEnumerable<BinType> binTypes, List<Item> items)
    {
        int totalVolume = items.Sum(i => i.Volume);

        var sortedBins = binTypes
            .OrderBy(bt => bt.Cost / bt.Volume)
            .ThenByDescending(bt => bt.Volume)
            .ToList();

        int accumulated = 0;
        foreach (var bin in sortedBins)
        {
            accumulated += bin.Volume;
            if (accumulated >= totalVolume)
            {
                return bin;
            }
        }

        // اگر هیچ مجموعه‌ای پاسخ نداد، حداقل بزرگ‌ترین را انتخاب کن
        return sortedBins.LastOrDefault();
    }
}