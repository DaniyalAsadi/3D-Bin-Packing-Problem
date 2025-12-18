using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.FitnessCalculator.Implementation;

/// <summary>
/// Calculates chromosome fitness by combining bin costs with penalties for unplaced items.
/// </summary>
public class DefaultFitnessCalculator(IPlacementAlgorithm placementAlgorithm) : IFitnessCalculator
{
    private readonly int _penaltyCoefficient = SettingsManager.Current.Genetic.PenaltyCoefficient;

    private readonly float _alpha = SettingsManager.Current.Genetic.AlphaWeight; // ضریب وزن‌دهی پرشدگی
    private readonly float _beta = SettingsManager.Current.Genetic.BetaWeight;  // ضریب وزن‌دهی هزینه



    public FitnessResultViewModel Evaluate(Chromosome chromosome, List<Item> items)
    {
        var results = placementAlgorithm.Execute(items, chromosome.Sequences.Select(e =>
            BinType.Create(
                e.BinType.Name,
                e.Length.Value,
                e.Width.Value,
                e.Height.Value,
                e.BinType.MaxWeight,
                e.BinType.Cost,
                e.BinType.TareWeight)).ToList());

        var fillRate = results.SpaceUtilization;
        var totalCost = results.UsedBinTypes.Sum(b => b.BinType.Cost);
        var leftItemsCount = results.LeftItems.Count;

        // ۱. نرمال‌سازی هزینه برای هم‌مقیاس شدن
        var maxPossibleCost = EstimateMaxPossibleCost(items, results.UsedBinTypes);
        var normalizedCost = (float)(totalCost / maxPossibleCost);

        // ۲. محاسبه fitness با فرمول صحیح
        var fitness = (_alpha * fillRate)                    // fillRate بالا → fitness بالا
                      + (_beta * (1 - normalizedCost))       // هزینه پایین → fitness بالا  
                      - (_penaltyCoefficient * leftItemsCount); // آیتم باقیمانده → fitness پایین

        return new FitnessResultViewModel()
        {
            PackingResults = results,
            Fitness = fitness,
        };
    }

    private static decimal EstimateMaxPossibleCost(List<Item> items, List<BinInstance> usedBins)
    {
        if (!usedBins.Any()) return 1;

        // بیشترین هزینه ممکن: همه آیتم‌ها در گران‌ترین جعبه
        var mostExpensiveBin = usedBins.Max(b => b.BinType.Cost);
        return items.Count * mostExpensiveBin;
    }
}
