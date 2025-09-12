using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Selection;
public interface ISelection
{
    /// <summary>
    /// Selects individuals for the next generation based on selection strategy.
    /// </summary>
    /// <param name="population">Current generation population.</param>
    /// <param name="nextGenerationSize">Target size of the next generation.</param>
    /// <param name="elitismPopulationSize">Number of elite individuals to preserve.</param>
    /// <returns>Next generation population.</returns>
    Chromosome Select(
        List<Chromosome> population,
        int nextGenerationSize,
        int elitismPopulationSize);
}