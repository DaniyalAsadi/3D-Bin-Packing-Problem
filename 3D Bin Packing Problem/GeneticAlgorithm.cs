using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm(List<Product> products, List<Box> boxes)
{
    private readonly Random _random = new();
    private readonly List<Product> _products = products;
    private readonly List<Box> _boxes = boxes;

    private List<Chromosome> population;
    private readonly int populationSize = 20;
    private readonly int generations = 1000;
    private readonly double mutationRate = 1;

    public Chromosome Execute()
    {
        InitializePopulation();
        while (population.Count < generations)
        {
            var parents = RouletteSelectionStrategy.Select(population, 2);
            var children = CrossOverStrategy.Crossover(parents[0], parents[1]);
            population.Add(children.Item1);
            population.Add(children.Item2);
            if (_random.NextDouble() < mutationRate)
            {
                if (children.Item1 is { Fitness: double.MaxValue })
                {
                    MutationStrategy.PositionShiftMutation(children.Item1);
                }
            }
            if (_random.NextDouble() < mutationRate)
            {
                if (children.Item2 is { Fitness: double.MaxValue })
                {
                    MutationStrategy.PositionShiftMutation(children.Item2);
                }
            }
        }
        return population.OrderBy(e => e.Fitness).First();
    }
    public void InitializePopulation()
    {
        population = new List<Chromosome>();
        for (int i = 0; i < populationSize; i++)
        {
            population.Add(new Chromosome(_products, _boxes));
        }
    }
}
