using _3D_Bin_Packing_Problem;
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
using Microsoft.Extensions.DependencyInjection;

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

// ساخت ServiceProvider
var serviceProvider = services.BuildServiceProvider();

// اجرای برنامه
var app = serviceProvider.GetRequiredService<GeneticAlgorithm>();
List<Item> products =
[
    new(2,2,2),
    new(2,2,2),
    new(2,2,2),
    new(2,2,2),
    new(2,2,2),
    new(2,2,2),
    new(2,2,2),
    new(2,2,2),
];
var x = app.Execute(products);

Console.WriteLine(x.PackingResults);