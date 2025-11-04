using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.FitnessCalculator;
public interface IFitnessCalculator
{
    /// <summary>
    /// Evaluates the fitness of a chromosome.
    /// Higher is better.
    /// </summary>
    FitnessResultViewModel Evaluate(Chromosome chromosome, List<Item> items);
}
