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
        double cost = results.UsedBinTypes.Sum(b => b.Cost);
        double penalty = results.LeftItems.Count * PenaltyCoefficient;
        var fitness = cost + penalty;
        return new FitnessResultViewModel()
        {
            PackingResults = results,
            Fitness = fitness,
        };
    }
}
