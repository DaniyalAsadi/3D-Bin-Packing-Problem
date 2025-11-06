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

    private readonly double _alpha = SettingsManager.Current.Genetic.AlphaWeight; // ضریب وزن‌دهی پرشدگی
    private readonly double _beta = SettingsManager.Current.Genetic.BetaWeight;  // ضریب وزن‌دهی هزینه




    public FitnessResultViewModel Evaluate(Chromosome chromosome, List<Item> items)
    {
        var results = placementAlgorithm.Execute(items, chromosome.GeneSequences.Select(e =>
            new BinType()
            {
                Height = e.Height.Value,
                Length = e.Length.Value,
                Width = e.Width.Value,
                Description = e.BinType.Description,
                CostFunc = () => e.BinType.Cost
            }).ToList());
        var totalCost = results.UsedBinTypes.Sum(b => b.BinType.Cost);

        // محاسبه FillRate
        var totalPackedVolume = items.Sum(item => item.Length * item.Width * item.Height); // حجم اقلام بسته‌بندی شده
        var totalBinVolume = results.UsedBinTypes.Sum(bin => bin.BinType.Length * bin.BinType.Width * bin.BinType.Height); // حجم کل جعبه‌ها
        int fillRate;
        if (totalBinVolume > 0)
        {
            fillRate = totalPackedVolume / totalBinVolume;
        }
        else
        {
            fillRate = 0; // یا مقدار مناسب دیگر
        }
        // محاسبه جریمه
        var penalty = results.LeftItems.Count * _penaltyCoefficient;

        // محاسبه Fitness
        var fitness = _alpha * fillRate - _beta * totalCost + penalty;

        return new FitnessResultViewModel()
        {
            PackingResults = results,
            Fitness = fitness,
        };
    }
}
