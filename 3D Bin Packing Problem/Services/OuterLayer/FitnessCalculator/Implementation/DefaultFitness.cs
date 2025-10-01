using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PA;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator.Implementation;
internal class DefaultFitness(IPlacementAlgorithm placementAlgorithm) : IFitness
{

    public double Evaluate(Chromosome chromosome, List<Item> items)
    {
        var fitness = placementAlgorithm.Execute(items, chromosome.GeneSequences.Select(e => e.BinType).ToList());
        chromosome.SetFitness(1000);
        return 1000;
    }
}
