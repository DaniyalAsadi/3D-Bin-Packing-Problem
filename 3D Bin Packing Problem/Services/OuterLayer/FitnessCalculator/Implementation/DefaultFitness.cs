using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator.Implementation;
internal class DefaultFitness : IFitness
{
    public double Evaluate(Chromosome chromosome)
    {
        return chromosome.GeneSequences.Sum(x => x.BinType.Cost);
    }
}
