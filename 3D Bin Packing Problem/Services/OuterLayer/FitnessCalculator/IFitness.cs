using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator;
public interface IFitness
{
    /// <summary>
    /// Evaluates the fitness of a chromosome.
    /// Higher is better.
    /// </summary>
    double Evaluate(Chromosome chromosome, List<Item> items);
}
