using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.Crossover;
using _3D_Bin_Packing_Problem.Services.Mutation;
using _3D_Bin_Packing_Problem.Services.Selection;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm(
    List<ICrossoverOperator> crossoverOperators,
    List<IMutationOperator> mutationOperators,
    ISelection selection)
{
    private const int MaxIteration = 100;
    private const int PopulationSize = 100;
    private const int ElitismPopulationSize = 10;
    private int _iter = 0;

    private List<Chromosome> _population = [];
    private List<Chromosome> _elitismPopulation = [];

    public Chromosome Execute(
        List<Product> itemList)
    {
        var initialPopulation = GenerateInitialPopulation(itemList);
        CalculateFitnessAndSort(initialPopulation);
        _population = initialPopulation;

        _elitismPopulation = initialPopulation.Take(ElitismPopulationSize).ToList();
        var bestIndividual = _population.First();
        _iter = 0;
        while (_iter < MaxIteration)
        {
            List<Chromosome> newPopulation = [];
            if (newPopulation == null) throw new ArgumentNullException(nameof(newPopulation));
            bestIndividual = _population[0];
            while (newPopulation.Count < PopulationSize)
            {
                var parentA = selection.Select(_population, PopulationSize * 2, ElitismPopulationSize);
                var parentB = selection.Select(_population, PopulationSize * 2, ElitismPopulationSize);

                List<Chromosome> crossChildrenList = [];
                if (crossChildrenList == null) throw new ArgumentNullException(nameof(crossChildrenList));
                foreach (var crossoverOperator in crossoverOperators)
                {
                    var (crossChildA, crossChildB) = crossoverOperator.Crossover(parentA, parentB);
                    crossChildrenList.Add(crossChildA);
                    crossChildrenList.Add(crossChildB);
                }
                crossChildrenList.Sort();
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
                muteChildrenList.Sort();
                newPopulation.Add(muteChildrenList[0]);
                newPopulation.Add(muteChildrenList[1]);

            }
            // Step 8: Replace population
            _population = _population.Except(_elitismPopulation).ToList();
            _population = _population.Concat(newPopulation).ToList();
            _population.Sort();

            _elitismPopulation = _population.Take(ElitismPopulationSize).ToList();

            if (_population.First().Fitness <= bestIndividual.Fitness)
            {
                bestIndividual = _population.First();
            }
            _iter++;
        }


        return bestIndividual;
    }

    private List<Chromosome> GenerateInitialPopulation(List<Product> itemList)
    {
        throw new NotImplementedException();
    }

    private void CalculateFitnessAndSort(List<Chromosome> list)
    {
        throw new NotImplementedException();
    }

}