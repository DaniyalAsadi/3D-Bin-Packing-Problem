using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services;

/// <summary>
/// Implements the roulette wheel selection strategy for selecting chromosomes based on their fitness.
/// </summary>
public class RouletteSelectionStrategy
{
    /// <summary>
    /// Selects a specified number of chromosomes from the given list using the roulette wheel selection method.
    /// </summary>
    /// <param name="chromosomes">The list of chromosomes to select from.</param>
    /// <param name="count">The number of chromosomes to select.</param>
    /// <returns>A list of selected chromosomes.</returns>
    public static List<Chromosome> Select(List<Chromosome> chromosomes, int count)
    {
        const double epsilon = 1e-6; // مقدار کوچک برای جلوگیری از تقسیم بر صفر
        var maxFitness = chromosomes.Max(x => x.Fitness);

        // محاسبه فیتنس معکوس شده
        var adjustedFitness = chromosomes
            .Select(x => (maxFitness - x.Fitness) + epsilon)
            .ToList();

        var totalAdjustedFitness = adjustedFitness.Sum();
        var selectedChromosomes = new List<Chromosome>();
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var randomValue = random.NextDouble() * totalAdjustedFitness;
            var currentSum = 0.0;

            for (int j = 0; j < chromosomes.Count; j++)
            {
                currentSum += adjustedFitness[j];
                if (currentSum >= randomValue)
                {
                    selectedChromosomes.Add(chromosomes[j]);
                    break;
                }
            }
        }

        return selectedChromosomes;
    }

}
