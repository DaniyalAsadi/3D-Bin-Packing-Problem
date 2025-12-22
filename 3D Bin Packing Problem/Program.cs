using _3D_Bin_Packing_Problem.Core;
using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Datasets;
using _3D_Bin_Packing_Problem.Core.Models;
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
    settings.Genetic.PopulationSize = 30;          // تعداد جمعیت
    settings.Genetic.MaxIteration = 50;           // حداکثر تعداد تکرار
    settings.Genetic.MutationRate = 0.05;          // نرخ جهش
    settings.Genetic.CrossoverRate = 0.85;         // نرخ ترکیب
    settings.Genetic.TournamentGroupSize = 5;      // اندازه گروه تورنومنت
    settings.Genetic.ElitismPopulationSize = 5;    // تعداد نخبگان
    settings.Genetic.SupportThreshold = 1;      // حداقل نسبت تکیه‌گاه
    settings.Genetic.AlphaWeight = 1.0f;            // ضریب آلفا در تابع برازندگی
    settings.Genetic.BetaWeight = 1.0f;             // ضریب بتا در تابع برازندگی
    settings.Genetic.PenaltyCoefficient = 20000; // جریمه آیتم‌های بسته‌نشده

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
var geneticAlgorithm = serviceProvider.GetRequiredService<GeneticAlgorithm>();


var bins = BinTypeDataset.StandardBinTypes();

List<Item> items = ItemDatasets.GeneticStressTest();


var startTime = DateTime.Now;
var chromosome = geneticAlgorithm.Execute(bins, items);
var executionTime = (DateTime.Now - startTime).TotalSeconds;

var packingResults = chromosome.PackingResults;
Console.WriteLine(packingResults);
Console.WriteLine("----------------------");
Console.WriteLine(executionTime);