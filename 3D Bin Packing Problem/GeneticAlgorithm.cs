using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm(List<Product> products, List<Box> boxes)
{
    private readonly Random _random = new();
    private readonly List<Product> _products = products;
    private readonly List<Box> _boxes = boxes;

    public List<Chromosome> Population { get; set; }
    private readonly int _populationSize = 10;
    private readonly int _generations = 100;
    private readonly double _mutationRate = 0.1;
    private readonly double _crossoverRate = 0.9;
    private readonly int _elitismCount = 5;

    public void Execute()
    {
        InitializePopulation();


    }
    public void InitializePopulation()
    {
        Population = new List<Chromosome>();
        for (int i = 0; i < _populationSize; i++)
        {
            Population.Add(new Chromosome(_products, _boxes));
        }
        foreach (var chromosome in Population)
        {
            Console.WriteLine(chromosome);
            Console.WriteLine("------------------");
        }
    }
}
