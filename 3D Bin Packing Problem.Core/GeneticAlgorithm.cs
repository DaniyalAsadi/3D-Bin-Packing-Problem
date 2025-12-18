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
using System.Threading.Tasks;

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
    private readonly ParallelOptions _parallelOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount };
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
        populationGenerator.SetAvailableBins(availableBinTypes);
        var binTypeCount = Math.Max(1, (int)(availableBinTypes.Count * 0.4));
        var population = populationGenerator.Generate(itemList, _populationSize, binTypeCount);

        Parallel.ForEach(population, _parallelOptions, chr =>
        {
            var res = fitnessCalculator.Evaluate(chr, itemList);
            chr.SetFitness(res);
        });

        population.Sort(comparer);
        var best = population[0].Clone();
        int noImprovement = 0;

        for (int iter = 0; iter < _maxIteration; iter++)
        {
            var newPopulation = new List<Chromosome>(_populationSize);

            // التیزم مستقیم
            newPopulation.AddRange(population.Take(_elitismPopulationSize).Select(c => c.Clone()));

            while (newPopulation.Count < _populationSize)
            {
                var parent1 = selection.Select(population, itemList, _tournamentGroupSize, _elitismPopulationSize).First();
                var parent2 = selection.Select(population, itemList, _tournamentGroupSize, _elitismPopulationSize).First();

                var offspring = _random.NextDouble() < _crossoverProbability
                    ? _crossoverOperators.SelectMany(op => op.Crossover(parent1, parent2)).ToList()
                    : [parent1.Clone(), parent2.Clone()];

                var mutated = offspring.Select(child =>
                    _random.NextDouble() < _mutationProbability
                        ? _mutationOperators.Aggregate(child.Clone(), (c, op) => op.Mutate(c))
                        : child.Clone()
                ).ToList();

                Parallel.ForEach(mutated, _parallelOptions, chr =>
                {
                    var res = fitnessCalculator.Evaluate(chr, itemList);
                    chr.SetFitness(res);
                });

                mutated.Sort(comparer);
                newPopulation.AddRange(mutated.Take(_populationSize - newPopulation.Count));
            }

            population = newPopulation;
            population.Sort(comparer);

            if (population[0].Fitness > best.Fitness)
            {
                best = population[0].Clone();
                noImprovement = 0;
            }
            else if (++noImprovement >= 20)
                break;
        }

        return best;
    }
}
