using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm(List<Product> products, List<Box> boxes)
{
    private readonly Random _random = new();
    private readonly List<Product> _products = products;
    private readonly List<Box> _boxes = boxes;

    public List<Chromosome> Population { get; set; }
    private readonly int _populationSize = 100;
    private readonly int _generations = 200;
    private readonly double _mutationRate = 0.1;

    public Chromosome Execute()
    {
        InitializePopulation();
        while(Population.Count < _generations)
        {
            var parents=  RouletteSelectionStrategy.Select(Population, 2);
            var children = CrossOverStrategy.Crossover(parents[0], parents[1]);
            Population.Add(children.Item1);
            Population.Add(children.Item2);
        }
        return Population.OrderBy(e => e.Fitness).First();
    }
    public void InitializePopulation()
    {
        Population = new List<Chromosome>();
        for (int i = 0; i < _populationSize; i++)
        {
            Population.Add(new Chromosome(_products, _boxes));
        }
    }
}
