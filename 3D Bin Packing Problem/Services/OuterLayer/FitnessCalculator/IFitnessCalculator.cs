using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.ViewModels;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator;
public interface IFitnessCalculator
{
    /// <summary>
    /// Evaluates the fitness of a chromosome.
    /// Higher is better.
    /// </summary>
    FitnessResultViewModel Evaluate(Chromosome chromosome, List<Item> items);
}
