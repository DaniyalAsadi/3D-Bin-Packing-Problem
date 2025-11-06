using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.FitnessCalculator;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.FitnessCalculator.Implementation;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation;
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
    ISelection selection,
    IFitnessCalculator fitnessCalculator,
    IComparer<Chromosome> comparer,
    CrossoverFactory crossoverFactory,
    MutationFactory mutationFactory)
{
    // Configurable constants
    private readonly int _maxIteration = SettingsManager.Current.Genetic.MaxIteration;
    private readonly int _populationSize = SettingsManager.Current.Genetic.PopulationSize;
    private readonly double _crossoverProbability = SettingsManager.Current.Genetic.CrossoverRate;   // احتمال کراس‌اور
    private readonly double _mutationProbability = SettingsManager.Current.Genetic.MutationRate;    // احتمال جهش
    private readonly int _tournamentGroupSize = SettingsManager.Current.Genetic.TournamentGroupSize;         // اندازه گروه انتخاب تورنمنتی
    private readonly int _elitismPopulationSize = SettingsManager.Current.Genetic.ElitismPopulationSize;

    private readonly IEnumerable<ICrossoverOperator> _crossoverOperators =
        crossoverFactory.Create(SettingsManager.Current.Crossover);

    private readonly IEnumerable<IMutationOperator> _mutationOperators =
        mutationFactory.Create(SettingsManager.Current.Mutation);

    private readonly Random _random = new();
    private List<Chromosome> _population = [];
    private List<Chromosome> _elitismPopulation = [];
    public static GeneticAlgorithm Default()
    {
        return new GeneticAlgorithm(
            new PopulationGenerator(),
            new RouletteWheelSelection(new ChromosomeFitnessComparer()),
            new DefaultFitnessCalculator(
                new PlacementAlgorithm(
                    new ItemOrderingStrategyFactory(),
                    new SubBinSelectionStrategyFactory(),
                    new SingleBinPackingAlgorithm(
                        new PlacementFeasibilityChecker(),
                        new SubBinUpdatingAlgorithm(),
                        new SubBinOrderingStrategyFactory()))),
            new ChromosomeFitnessComparer(),
            new CrossoverFactory(),
            new MutationFactory()
        );
    }

    public Chromosome Execute(List<BinType> availableBinTypes, List<Item> itemList)
    {


        // Step 1: Initial population
        populationGenerator.SetAvailableBins(availableBinTypes);
        var binTypeCount = Math.Max(1, (int)(availableBinTypes.Count * 0.4));
        var initialPopulation = populationGenerator.Generate(itemList, _populationSize, binTypeCount);
        initialPopulation.ForEach(x =>
        {
            var fitnessResultViewModel = fitnessCalculator.Evaluate(x, itemList);
            x.SetFitness(fitnessResultViewModel);
        });
        initialPopulation.Sort(comparer);
        _population = initialPopulation;

        _elitismPopulation = initialPopulation.Take(_elitismPopulationSize).ToList();
        var bestIndividual = _population.First();
        var iter = 0;

        while (iter < _maxIteration)
        {
            List<Chromosome> newPopulation = [];

            bestIndividual = _population[0];

            while (newPopulation.Count < _populationSize)
            {
                // Step 3: Selection
                var parentA = selection.Select(_population, itemList, _tournamentGroupSize, _elitismPopulationSize).First();
                var parentB = selection.Select(_population, itemList, _tournamentGroupSize, _elitismPopulationSize).First();
                // Step 4: Crossover with probability
                var crossChildrenList = new List<Chromosome>();
                if (_random.NextDouble() < _crossoverProbability)
                {
                    foreach (var crossoverOperator in _crossoverOperators)
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
                if (_random.NextDouble() < _mutationProbability)
                {
                    foreach (var mutationOperator in _mutationOperators)
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
            _population = _population.Concat(_elitismPopulation).Concat(newPopulation.Take(_populationSize - _elitismPopulationSize)).ToList();
            _population.Sort(comparer);

            _elitismPopulation = _population.Take(_elitismPopulationSize).ToList();


            if (_population.First().Fitness <= bestIndividual.Fitness)
            {
                bestIndividual = _population.First();

            }

            iter++;
        }
        return bestIndividual;
    }
}
