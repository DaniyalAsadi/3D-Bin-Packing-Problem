using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator.Implementation;

public class DefaultFitnessCalculator(IPlacementAlgorithm placementAlgorithm) : IFitnessCalculator
{
    private const int PenaltyCoefficient = 1000;

    public FitnessResultViewModel Evaluate(Chromosome chromosome, List<Item> items)
    {
        var results = placementAlgorithm.Execute(items, chromosome.GeneSequences.Select(e => e.BinType).ToList());
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
