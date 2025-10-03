using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Crossover;
using _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Mutation;
using _3D_Bin_Packing_Problem.Services.OuterLayer.PopulationGenerator;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Selection;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm(
    IPopulationGenerator populationGenerator,
    IEnumerable<ICrossoverOperator> crossoverOperators,
    IEnumerable<IMutationOperator> mutationOperators,
    ISelection selection,
    IFitnessCalculator fitnessCalculator,
    IComparer<Chromosome> comparer)
{
    // Configurable constants
    private const int MaxIteration = 50;
    private const int PopulationSize = 30;
    private const double CrossoverProbability = 0.7;   // احتمال کراس‌اور
    private const double MutationProbability = 0.2;    // احتمال جهش
    private const int TournamentGroupSize = 8;         // اندازه گروه انتخاب تورنمنتی
    private const int ElitismPopulationSize = 5;

    private readonly Random _random = new();
    private List<Chromosome> _population = [];
    private List<Chromosome> _elitismPopulation = [];

    public Chromosome Execute(List<Item> itemList)
    {
        // Step 1: Initial population
        var initialPopulation = populationGenerator.Generate(itemList, PopulationSize, 1);
        initialPopulation.ForEach(x =>
        {
            var fitnessResultViewModel = fitnessCalculator.Evaluate(x, itemList);
            x.SetFitness(fitnessResultViewModel);
        });
        initialPopulation.Sort(comparer);
        _population = initialPopulation;

        _elitismPopulation = initialPopulation.Take(ElitismPopulationSize).ToList();
        var bestIndividual = _population.First();
        var iter = 0;

        while (iter < MaxIteration)
        {
            List<Chromosome> newPopulation = [];

            bestIndividual = _population[0];

            while (newPopulation.Count < PopulationSize)
            {
                // Step 3: Selection
                var parentA = selection.Select(_population, itemList, TournamentGroupSize, ElitismPopulationSize).First();
                var parentB = selection.Select(_population, itemList, TournamentGroupSize, ElitismPopulationSize).First();

                // Step 4: Crossover with probability
                var crossChildrenList = new List<Chromosome>();
                if (_random.NextDouble() < CrossoverProbability)
                {
                    foreach (var crossoverOperator in crossoverOperators)
                    {
                        var (crossChildA, crossChildB) = crossoverOperator.Crossover(parentA, parentB);
                        crossChildrenList.Add(crossChildA);
                        crossChildrenList.Add(crossChildB);
                    }
                }
                else
                {
                    crossChildrenList.Add(parentA.Clone());
                    crossChildrenList.Add(parentB.Clone());
                }
                crossChildrenList.ForEach(x =>
                {
                    if (x.Fitness.HasValue) return;
                    var fitnessResultViewModel = fitnessCalculator.Evaluate(x, itemList);
                    x.SetFitness(fitnessResultViewModel);
                });
                crossChildrenList.Sort(comparer);
                var childA = crossChildrenList[0];
                var childB = crossChildrenList[1];

                // Step 5: Mutation with probability
                var muteChildrenList = new List<Chromosome>();
                if (_random.NextDouble() < MutationProbability)
                {
                    foreach (var mutationOperator in mutationOperators)
                    {
                        var muteChildA = mutationOperator.Mutate(childA);
                        var muteChildB = mutationOperator.Mutate(childB);
                        muteChildrenList.Add(muteChildA);
                        muteChildrenList.Add(muteChildB);
                    }
                }
                else
                {
                    muteChildrenList.Add(childA.Clone());
                    muteChildrenList.Add(childB.Clone());
                }
                muteChildrenList.ForEach(x =>
                {
                    if (x.Fitness.HasValue) return;
                    var fitnessResultViewModel = fitnessCalculator.Evaluate(x, itemList);
                    x.SetFitness(fitnessResultViewModel);
                });
                muteChildrenList.Sort(comparer);
                newPopulation.Add(muteChildrenList[0]);
                newPopulation.Add(muteChildrenList[1]);
            }

            // Step 8: Replace population
            _population = _population.Except(_elitismPopulation).ToList();
            _population = _population.Concat(newPopulation).ToList();
            _population.Sort(comparer);

            _elitismPopulation = _population.Take(ElitismPopulationSize).ToList();


            if (_population.First().Fitness <= bestIndividual.Fitness)
            {
                bestIndividual = _population.First();
            }

            iter++;
        }

        return bestIndividual;
    }
}
