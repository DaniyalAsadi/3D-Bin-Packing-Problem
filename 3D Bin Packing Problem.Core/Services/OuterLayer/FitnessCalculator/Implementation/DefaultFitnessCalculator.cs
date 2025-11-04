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
    private const int PenaltyCoefficient = 200000;
    private double _alpha; // ضریب وزن‌دهی پرشدگی
    private double _beta;  // ضریب وزن‌دهی هزینه
    public void SetAlpha(double alpha)
    {
        // تنظیم ضریب وزن‌دهی پرشدگی
        _alpha = alpha;
    }
    public void SetBeta(double beta)
    {
        // تنظیم ضریب وزن‌دهی هزینه
        _beta = beta;
    }



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
        var totalCost = results.UsedBinTypes.Sum(b => b.Cost);

        // محاسبه FillRate
        var totalPackedVolume = items.Sum(item => item.Length * item.Width * item.Height); // حجم اقلام بسته‌بندی شده
        var totalBinVolume = results.UsedBinTypes.Sum(bin => bin.Length * bin.Width * bin.Height); // حجم کل جعبه‌ها
        var fillRate = totalPackedVolume / totalBinVolume;

        // محاسبه جریمه
        var penalty = results.LeftItems.Count * PenaltyCoefficient;

        // محاسبه Fitness
        var fitness = _alpha * fillRate - _beta * totalCost + penalty;

        return new FitnessResultViewModel()
        {
            PackingResults = results,
            Fitness = fitness,
        };
    }
}
