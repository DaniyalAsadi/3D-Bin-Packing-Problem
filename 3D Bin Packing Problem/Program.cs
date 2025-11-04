using _3D_Bin_Packing_Problem.Core;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(builder =>
{
    builder.AddConsole();          // نمایش در Console
    builder.SetMinimumLevel(LogLevel.Debug); // حداقل سطح لاگ
});
services.AddScoped<GeneticAlgorithm>();
services.AddScoped<IPopulationGenerator, PopulationGenerator>();
services.AddScoped<ISelection, RouletteWheelSelection>();
services.AddScoped<IFitnessCalculator, DefaultFitnessCalculator>();

services.AddScoped<ICrossoverOperator, OnePointCrossover>();
services.AddScoped<ICrossoverOperator, TwoPointCrossover>();
services.AddScoped<ICrossoverOperator, UniformCrossover>();
services.AddScoped<ICrossoverOperator, MultiPointCrossover>();


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

//var PresetBinTypes = new List<BinType>
//{
//    new() { Description = "Size 1", Length = 150, Width = 100, Height = 100, CostFunc = () => 63800 },
//    new() { Description = "Size 2", Length = 200, Width = 150, Height = 100, CostFunc = () => 115500 },
//    new() { Description = "Size 3", Length = 200, Width = 200, Height = 150, CostFunc = () => 172700 },
//    new() { Description = "Size 4", Length = 300, Width = 200, Height = 200, CostFunc = () => 247500 },
//    new() { Description = "Size 5", Length = 350, Width = 250, Height = 200, CostFunc = () => 446600 },
//    new() { Description = "Size 6", Length = 450, Width = 250, Height = 200, CostFunc = () => 559900 },
//    new() { Description = "Size 7", Length = 400, Width = 300, Height = 250, CostFunc = () => 686400 },
//    new() { Description = "Size 8", Length = 450, Width = 400, Height = 300, CostFunc = () => 1043900 },
//    new() { Description = "Size 9", Length = 550, Width = 450, Height = 350, CostFunc = () => 1375000 },
//};
//List<Item> products =
//[
//    new(100,100,100),
//    new(100,50,100),
//];
var PresetBinTypes = new List<BinType>
{
    new() { Description = "Size 1", Length = 4, Width = 4, Height = 4, CostFunc = () => 63800 },

};
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
    new(2,2,2),
];

var x = app.Execute(PresetBinTypes, products);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(x.PackingResults);
Console.ForegroundColor = ConsoleColor.Gray;
