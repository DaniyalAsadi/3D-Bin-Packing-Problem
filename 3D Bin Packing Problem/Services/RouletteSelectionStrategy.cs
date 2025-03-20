using _3D_Bin_Packing_Problem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var totalFitness = chromosomes.Sum(x => x.Fitness);
        var selectedChromosomes = new List<Chromosome>();
        var random = new Random();
        for (int i = 0; i < count; i++)
        {
            var randomValue = random.NextDouble() * totalFitness;
            var currentSum = 0.0;
            foreach (var chromosome in chromosomes)
            {
                currentSum += chromosome.Fitness;
                if (currentSum >= randomValue)
                {
                    selectedChromosomes.Add(chromosome);
                    break;
                }
            }
        }
        return selectedChromosomes;
    }
}
