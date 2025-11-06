using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Selection.Implementation;

/// <summary>
/// Selects chromosomes for the next generation using roulette wheel probability while preserving elites.
/// </summary>
public class RouletteWheelSelection(IComparer<Chromosome> comparer) : ISelection
{
    private readonly Random _random = new Random();

    public List<Chromosome> Select(
        List<Chromosome> population,
        List<Item> items,
        int nextGenerationSize,
        int elitismPopulationSize)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population must not be empty.");

        if (elitismPopulationSize > nextGenerationSize)
            throw new ArgumentException("Elitism size cannot exceed next generation size.");


        // 2. Sort by fitness (descending) and preserve elites
        var elites = population
            .OrderByDescending(e => e.Fitness)
            .Take(elitismPopulationSize)
            .ToList();


        // 3. Compute total fitness for roulette wheel
        var totalFitness = population.Sum(e => e.Fitness);
        if (totalFitness <= 0)
            totalFitness = AppConstants.Tolerance; // avoid division by zero

        var selected = new List<Chromosome>(elites);

        // 4. Roulette wheel selection for the rest
        for (var i = elites.Count; i < nextGenerationSize; i++)
        {
            var spin = _random.NextDouble() * totalFitness;
            double cumulative = 0;

            foreach (var e in population)
            {
                cumulative += e.Fitness;
                if (!(cumulative >= spin)) continue;
                selected.Add(e);
                break;
            }
        }

        selected.Sort(comparer);
        return selected; // 🔹 حالا کل لیست برمی‌گرده
    }
}


