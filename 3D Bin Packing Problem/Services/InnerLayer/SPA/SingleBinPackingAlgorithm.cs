using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;

public class SingleBinPackingAlgorithm(
    IPlacementFeasibilityChecker feasibilityChecker,
    ISubBinUpdatingAlgorithm subBinUpdatingAlgorithm,
    ISubBinOrderingStrategy subBinOrderingStrategy // 🔹 استراتژی مرتب‌سازی SubBin
) : ISingleBinPackingAlgorithm
{
    public PackingResultViewModel Execute(List<Item> items, BinType binType)
    {
        var itemList = items.ToList();
        var subBinList = new List<SubBin> { binType };
        var leftItemList = new List<Item>();
        var packedItemList = new List<PlacementResult>();

        foreach (var item in itemList.ToList())
        {
            // 🔹 استفاده از ApplySpeedUpStrategy به جای شرط inline
            var validSubBins = ApplySpeedUpStrategy(subBinList, new List<Item> { item });

            if (!validSubBins.Any())
            {
                leftItemList.Add(item);
                itemList.Remove(item);
                continue;
            }

            // 🔹 مرتب‌سازی SubBinها بر اساس استراتژی انتخابی (S1..S5)
            validSubBins = subBinOrderingStrategy.Apply(validSubBins, item).ToList();

            bool placed = false;

            foreach (var validSubBin in validSubBins)
            {
                if (feasibilityChecker.Execute(item, validSubBin, out var placementResult))
                {
                    ArgumentNullException.ThrowIfNull(placementResult);

                    packedItemList.Add(placementResult);

                    // 🔹 آپدیت SubBin باید بر اساس placementResult انجام شود، نه فقط item
                    subBinUpdatingAlgorithm.Execute(subBinList, placementResult);

                    placed = true;
                    break; // اولین SubBin معتبر انتخاب می‌شود
                }
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
                Height = x.Height,
                Length = x.Length,
                Width = x.Width,
            }).ToList(),

            PackedItems = packedItemList.Select(x => new PackedItemViewModel
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
    private List<SubBin> ApplySpeedUpStrategy(List<SubBin> subBins, List<Item> items)
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
