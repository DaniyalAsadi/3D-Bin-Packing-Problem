using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Selection;
public interface ISelection
{
    /// <summary>
    /// Selects individuals for the next generation based on selection strategy.
    /// </summary>
    /// <param name="population">Current generation population.</param>
    /// <param name="items">Current item for packing</param>
    /// <param name="nextGenerationSize">Target size of the next generation.</param>
    /// <param name="elitismPopulationSize">Number of elite individuals to preserve.</param>
    /// <returns>Next generation population.</returns>
    List<Chromosome> Select(
        List<Chromosome> population,
        List<Item> items,
        int nextGenerationSize,
        int elitismPopulationSize);
}