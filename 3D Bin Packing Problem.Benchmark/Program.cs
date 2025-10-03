using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Crossover;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Crossover.Implementation;
using _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator;
using _3D_Bin_Packing_Problem.Services.OuterLayer.FitnessCalculator.Implementation;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Mutation;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Mutation.Implementation;
using _3D_Bin_Packing_Problem.Services.OuterLayer.PopulationGenerator;
using _3D_Bin_Packing_Problem.Services.OuterLayer.PopulationGenerator.Implementation;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Selection;
using _3D_Bin_Packing_Problem.Services.OuterLayer.Selection.Implementation;

namespace _3D_Bin_Packing_Problem.Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

public class GeneticAlgorithmBenchmark
{
    private ServiceProvider _serviceProvider = null!;
    private GeneticAlgorithm _ga = null!;
    private List<Item> _items = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Register services (same as your app code)
        var services = new ServiceCollection();

        services.AddScoped<GeneticAlgorithm>();
        services.AddScoped<IPopulationGenerator, PopulationGenerator>();
        services.AddScoped<ISelection, RouletteWheelSelection>();
        services.AddScoped<IFitnessCalculator, DefaultFitnessCalculator>();

        services.AddScoped<ICrossoverOperator, OnePointSwap>();
        services.AddScoped<ICrossoverOperator, TwoPointSwap>();
        services.AddScoped<ICrossoverOperator, SequenceReplacement>();
        services.AddScoped<ICrossoverOperator, SequenceSwap>();

        services.AddScoped<IMutationOperator, OnePointMutation>();
        services.AddScoped<IMutationOperator, TwoPointMutation>();

        services.AddScoped<IComparer<Chromosome>, ChromosomeFitnessComparer>();

        services.AddScoped<IPlacementAlgorithm, PlacementAlgorithm>();
        services.AddScoped<IItemOrderingStrategy, ItemOrderingStrategyI1>();
        services.AddScoped<ISubBinOrderingStrategy, SubBinOrderingStrategyS1>();
        services.AddScoped<ISubBinSelectionStrategy, SubBinSelectionStrategyB1>();
        services.AddScoped<ISingleBinPackingAlgorithm, SingleBinPackingAlgorithm>();
        services.AddScoped<IPlacementFeasibilityChecker, PlacementFeasibilityChecker>();
        services.AddScoped<ISubBinUpdatingAlgorithm, SubBinUpdatingAlgorithm>();

        _serviceProvider = services.BuildServiceProvider();

        // Resolve GeneticAlgorithm
        _ga = _serviceProvider.GetRequiredService<GeneticAlgorithm>();

        // Sample input items
        _items = new List<Item>
        {
            new Item(2, 2, 2),
        };
    }

    [Benchmark]
    public Chromosome RunGA()
    {
        return _ga.Execute(_items);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<GeneticAlgorithmBenchmark>();
    }
}

