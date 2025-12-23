using _3D_Bin_Packing_Problem.Core.Models;
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

        // 1. Elitism
        var elites = population
            .OrderByDescending(e => e.Fitness)
            .Take(elitismPopulationSize)
            .ToList();

        var selected = new List<Chromosome>(elites);

        // 2. Compute total fitness
        var totalFitness = population.Sum(e => e.Fitness);

        // If all fitness values are zero → fallback to uniform random selection
        if (totalFitness <= 0)
        {
            while (selected.Count < nextGenerationSize)
            {
                var randomIndex = _random.Next(population.Count);
                selected.Add(population[randomIndex]);
            }

            selected.Sort(comparer);
            return selected;
        }

        // 3. Roulette wheel selection
        for (var i = elites.Count; i < nextGenerationSize; i++)
        {
            var spin = _random.NextDouble() * totalFitness;
            double cumulative = 0;

            foreach (var e in population)
            {
                cumulative += e.Fitness;
                if (cumulative >= spin)
                {
                    selected.Add(e);
                    break;
                }
            }
        }

        selected.Sort(comparer);
        return selected;
    }
}
