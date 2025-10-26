using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover.Implementation;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.FitnessCalculator;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.FitnessCalculator.Implementation;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation.Implementation;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.PopulationGenerator;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.PopulationGenerator.Implementation;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Selection;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Selection.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
namespace _3D_Bin_Packing_Problem.Core;

/// <summary>
/// Coordinates the genetic algorithm workflow including population generation, selection, crossover, and mutation.
/// </summary>
public class GeneticAlgorithm(
    IPopulationGenerator populationGenerator,
    IEnumerable<ICrossoverOperator> crossoverOperators,
    IEnumerable<IMutationOperator> mutationOperators,
    ISelection selection,
    IFitnessCalculator fitnessCalculator,
    IComparer<Chromosome> comparer)
{
    // Configurable constants
    private const int MaxIteration = 10;
    private const int PopulationSize = 1000;
    private const double CrossoverProbability = 0.7;   // احتمال کراس‌اور
    private const double MutationProbability = 0.2;    // احتمال جهش
    private const int TournamentGroupSize = 8;         // اندازه گروه انتخاب تورنمنتی
    private const int ElitismPopulationSize = 5;

    private readonly Random _random = new();
    private List<Chromosome> _population = [];
    private List<Chromosome> _elitismPopulation = [];
    public static GeneticAlgorithm Default()
    {
        return new GeneticAlgorithm(
            new PopulationGenerator(),
            [new OnePointSwap(), new TwoPointSwap(), new SequenceReplacement(), new SequenceSwap()],
            [new OnePointMutation(), new TwoPointMutation()],
            new RouletteWheelSelection(new ChromosomeFitnessComparer()),
            new DefaultFitnessCalculator(
                new PlacementAlgorithm(
                    new ItemOrderingStrategyI1(),
                    new SubBinSelectionStrategyB1(),
                    new SingleBinPackingAlgorithm(
                        new PlacementFeasibilityChecker(),
                        new SubBinUpdatingAlgorithm(),
                        new SubBinOrderingStrategyS1()))),
            new ChromosomeFitnessComparer()
        );
    }

    public Chromosome Execute(List<BinType> availableBinTypes, List<Item> itemList)
    {
        // Step 1: Initial population
        populationGenerator.SetAvailableBins(availableBinTypes);
        int binTypeCount = Math.Max(1, (int)(availableBinTypes.Count * 0.4));
        var initialPopulation = populationGenerator.Generate(itemList, PopulationSize, binTypeCount);
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
                    var fitnessResultViewModel = fitnessCalculator.Evaluate(x, itemList);
                    x.SetFitness(fitnessResultViewModel);
                });
                muteChildrenList.Sort(comparer);
                newPopulation.Add(muteChildrenList[0]);
                newPopulation.Add(muteChildrenList[1]);
            }

            // Step 8: 
            _population = _population.Except(_elitismPopulation).ToList();
            _population = _population.Concat(_elitismPopulation).Concat(newPopulation.Take(PopulationSize - ElitismPopulationSize)).ToList();
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
