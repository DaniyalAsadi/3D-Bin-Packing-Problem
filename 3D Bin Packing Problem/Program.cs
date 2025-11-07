using _3D_Bin_Packing_Problem.Core;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
SettingsManager.Initialize(configure: settings =>
{
    // Genetic Algorithm Settings
    settings.Genetic.PopulationSize = 50;          // تعداد جمعیت
    settings.Genetic.MaxIteration = 100;           // حداکثر تعداد تکرار
    settings.Genetic.MutationRate = 0.05;          // نرخ جهش
    settings.Genetic.CrossoverRate = 0.85;         // نرخ ترکیب
    settings.Genetic.TournamentGroupSize = 5;      // اندازه گروه تورنومنت
    settings.Genetic.ElitismPopulationSize = 5;    // تعداد نخبگان
    settings.Genetic.SupportThreshold = 0.75;      // حداقل نسبت تکیه‌گاه
    settings.Genetic.AlphaWeight = 1.0;            // ضریب آلفا در تابع برازندگی
    settings.Genetic.BetaWeight = 1.0;             // ضریب بتا در تابع برازندگی
    settings.Genetic.PenaltyCoefficient = 200000; // جریمه آیتم‌های بسته‌نشده

    // Packing Settings
    settings.ItemOrdering = ItemOrderingStrategyType.I1;       // استراتژی مرتب‌سازی آیتم‌ها
    settings.SubBinOrdering = SubBinOrderingStrategyType.S1;   // استراتژی مرتب‌سازی زیرجعبه‌ها
    settings.SubBinSelection = SubBinSelectionStrategyType.B1; // استراتژی انتخاب زیرجعبه
    settings.Crossover = CrossoverType.All;                    // نوع Crossover
    settings.Mutation = MutationType.All;                      // نوع Mutation
});


services.AddLogging(builder =>
{
    builder.AddConsole();          // نمایش در Console
    builder.SetMinimumLevel(LogLevel.Debug); // حداقل سطح لاگ
});
services.AddScoped<GeneticAlgorithm>();
services.AddScoped<IPopulationGenerator, PopulationGenerator>();
services.AddScoped<ISelection, RouletteWheelSelection>();
services.AddScoped<IFitnessCalculator, DefaultFitnessCalculator>();

services.AddSingleton<ItemOrderingStrategyFactory>();
services.AddSingleton<SubBinOrderingStrategyFactory>();
services.AddSingleton<SubBinSelectionStrategyFactory>();

services.AddSingleton<CrossoverFactory>();
services.AddSingleton<MutationFactory>();

services.AddScoped<IComparer<Chromosome>, ChromosomeFitnessComparer>();


services.AddScoped<IPlacementAlgorithm, PlacementAlgorithm>();
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
var PresetBinTypes = new List<BinType>
{
    new() { Description = "Size 1", Length = 20, Width = 20, Height = 20, CostFunc = () => 63800 },

};

List<Item> products =
[
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
    new(10,10,10),
];

var x = app.Execute(PresetBinTypes, products);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(x.PackingResults);
Console.ForegroundColor = ConsoleColor.Gray;

