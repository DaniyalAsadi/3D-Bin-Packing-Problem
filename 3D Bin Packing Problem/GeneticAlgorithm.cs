using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.Crossover;
using _3D_Bin_Packing_Problem.Services.FitnessCalculator;
using _3D_Bin_Packing_Problem.Services.Mutation;
using _3D_Bin_Packing_Problem.Services.PopulationGenerator;
using _3D_Bin_Packing_Problem.Services.Selection;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm(
    IPopulationGenerator populationGenerator,
    IEnumerable<ICrossoverOperator> crossoverOperators,
    IEnumerable<IMutationOperator> mutationOperators,
    ISelection selection,
    IFitness fitness,
    IComparer<Chromosome> comparer)
{
    private const int MaxIteration = 100;
    private const int PopulationSize = 100;
    private const int ElitismPopulationSize = 10;

    private List<Chromosome> _population = [];
    private List<Chromosome> _elitismPopulation = [];

    public Chromosome Execute(
            List<Product> itemList)
    {
        var initialPopulation = populationGenerator.Generate(itemList, PopulationSize, 1);
        initialPopulation.Sort(comparer);
        _population = initialPopulation;

        _elitismPopulation = initialPopulation.Take(ElitismPopulationSize).ToList();
        var bestIndividual = _population.First();
        var iter = 0;
        while (iter < MaxIteration)
        {
            List<Chromosome> newPopulation = [];
            if (newPopulation == null) throw new ArgumentNullException(nameof(newPopulation));
            bestIndividual = _population[0];
            while (newPopulation.Count < PopulationSize)
            {
                var parentA = selection.Select(_population, PopulationSize, ElitismPopulationSize).First();
                var parentB = selection.Select(_population, PopulationSize, ElitismPopulationSize).First();

                List<Chromosome> crossChildrenList = [];
                if (crossChildrenList == null) throw new ArgumentNullException(nameof(crossChildrenList));
                foreach (var crossoverOperator in crossoverOperators)
                {
                    var (crossChildA, crossChildB) = crossoverOperator.Crossover(parentA, parentB);
                    crossChildrenList.Add(crossChildA);
                    crossChildrenList.Add(crossChildB);
                }
                crossChildrenList.Sort(comparer);
                var childA = crossChildrenList[0];
                var childB = crossChildrenList[1];

                List<Chromosome> muteChildrenList = [];
                if (muteChildrenList == null) throw new ArgumentNullException(nameof(muteChildrenList));

                foreach (var mutationOperator in mutationOperators)
                {
                    var muteChildA = mutationOperator.Mutate(childA);
                    var muteChildB = mutationOperator.Mutate(childB);
                    muteChildrenList.Add(muteChildA);
                    muteChildrenList.Add(muteChildB);
                }
                muteChildrenList.Sort(comparer);
                newPopulation.Add(muteChildrenList[0]);
                newPopulation.Add(muteChildrenList[1]);

            }
            // Step 8: Replace population
            _population = _population.Except(_elitismPopulation).ToList();
            _population = _population.Concat(newPopulation).ToList();
            _population.Sort(comparer);

            _elitismPopulation = _population.Take(ElitismPopulationSize).ToList();

            if (fitness.Evaluate(_population.First()) <= fitness.Evaluate(bestIndividual))
            {
                bestIndividual = _population.First();
            }
            iter++;
        }
        return bestIndividual;
    }


}