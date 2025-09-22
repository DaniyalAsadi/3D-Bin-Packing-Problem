using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Selection;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.Selection.Implementation;
internal class RouletteWheelSelection(IFitness fitnessCalculator, IComparer<Chromosome> comparer) : ISelection
{
    private readonly Random _random = new Random();

    public List<Chromosome> Select(
        List<Chromosome> population,
        int nextGenerationSize,
        int elitismPopulationSize)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population must not be empty.");

        if (elitismPopulationSize > nextGenerationSize)
            throw new ArgumentException("Elitism size cannot exceed next generation size.");

        // 1. Evaluate fitness for all individuals
        var evaluated = population
            .Select(c => new { Chromosome = c, Fitness = fitnessCalculator.Evaluate(c) })
            .ToList();

        // 2. Sort by fitness (descending) and preserve elites
        var elites = evaluated
            .OrderByDescending(e => e.Fitness)
            .Take(elitismPopulationSize)
            .Select(e => e.Chromosome)
            .ToList();

        // 3. Compute total fitness for roulette wheel
        double totalFitness = evaluated.Sum(e => e.Fitness);
        if (totalFitness <= 0)
            totalFitness = 1e-6; // avoid division by zero

        var selected = new List<Chromosome>(elites);

        // 4. Roulette wheel selection for the rest
        for (int i = elites.Count; i < nextGenerationSize; i++)
        {
            double spin = _random.NextDouble() * totalFitness;
            double cumulative = 0;

            foreach (var e in evaluated)
            {
                cumulative += e.Fitness;
                if (cumulative >= spin)
                {
                    selected.Add(e.Chromosome);
                    break;
                }
            }
        }

        selected.Sort(comparer);
        return selected; // 🔹 حالا کل لیست برمی‌گرده
    }
}


