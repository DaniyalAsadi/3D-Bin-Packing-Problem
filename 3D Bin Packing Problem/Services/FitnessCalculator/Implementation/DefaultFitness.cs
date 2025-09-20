using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.FitnessCalculator.Implementation;
internal class DefaultFitness : IFitness
{
    public double Evaluate(Chromosome chromosome)
    {
        return chromosome.Count;
    }
}
