using _3D_Bin_Packing_Problem.Models;

namespace _3D_Bin_Packing_Problem;

public class HyperparameterTuner
{
    private List<Product> _products;
    private List<Box> _availableBoxes;

    private readonly List<int> populationSizes = new List<int> { 30, 50, 100, 200 };
    private readonly List<int> generationsList = new List<int> { 30, 50, 100, 200 };
    private readonly List<double> mutationRates = new List<double> { 0.1, 0.15, 0.2, 0.25, 0.3 };

    public HyperparameterTuner(List<Product> products, List<Box> availableBoxes)
    {
        _products = products;
        _availableBoxes = availableBoxes;
    }

    public void RunGridSearch()
    {
        double bestFitness = double.MaxValue;
        (int popSize, int generations, double mutationRate) bestParams = (0, 0, 0.0);

        foreach (var popSize in populationSizes)
        {
            foreach (var generations in generationsList)
            {
                foreach (var mutationRate in mutationRates)
                {
                    Console.WriteLine($"Testing: Population={popSize}, Generations={generations}, MutationRate={mutationRate}");

                    // Initialize genetic algorithm with current parameters
                    var ga = new GeneticAlgorithm(_products, _availableBoxes, 0.5, 2.0, 1.5, 0.5);

                    ga.SetHyperParameter(popSize, generations, mutationRate);



                    // Run the algorithm
                    var bestChromosome = ga.Run();

                    if (bestChromosome.Fitness < bestFitness)
                    {
                        bestFitness = bestChromosome.Fitness;
                        bestParams = (popSize, generations, mutationRate);
                    }
                }
            }
        }

        Console.WriteLine($"\nBest Hyperparameters: Population={bestParams.popSize}, Generations={bestParams.generations}, MutationRate={bestParams.mutationRate}");
    }
}