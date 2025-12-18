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
var placementAlgorithm = serviceProvider.GetRequiredService<IPlacementAlgorithm>();

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
//var PresetBinTypes = new List<BinType>
//{
//    new() { Description = "Size 1", Length = 20, Width = 20, Height = 20, CostFunc = () => 63800 },

//};

//List<Item> products =
//[
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//];

//var x = app.Execute(PresetBinTypes, products);

//Console.ForegroundColor = ConsoleColor.Cyan;
//Console.WriteLine(x.PackingResults);
//Console.ForegroundColor = ConsoleColor.Gray;

// در متد اصلی، بعد از کدهای فعلی:

var bins = new List<BinType>
{
    BinType.Create(
        name: "Size 1",
        length: 150,
        width: 100,
        height: 100,
        maxWeight: 20,
        cost: 63_800m
    ),

    BinType.Create(
        name: "Size 2",
        length: 200,
        width: 150,
        height: 100,
        maxWeight: 30,
        cost: 115_500m
    ),

    BinType.Create(
        name: "Size 3",
        length: 200,
        width: 200,
        height: 150,
        maxWeight: 40,
        cost: 172_700m
    ),

    BinType.Create(
        name: "Size 4",
        length: 300,
        width: 200,
        height: 200,
        maxWeight: 50,
        cost: 247_500m
    ),

    BinType.Create(
        name: "Size 5",
        length: 350,
        width: 250,
        height: 200,
        maxWeight: 60,
        cost: 446_600m
    ),

    BinType.Create(
        name: "Size 6",
        length: 450,
        width: 250,
        height: 200,
        maxWeight: 70,
        cost: 559_900m
    ),

    BinType.Create(
        name: "Size 7",
        length: 400,
        width: 300,
        height: 250,
        maxWeight: 80,
        cost: 686_400m
    ),

    BinType.Create(
        name: "Size 8",
        length: 450,
        width: 400,
        height: 300,
        maxWeight: 100,
        cost: 1_043_900m
    ),

    BinType.Create(
        name: "Size 9",
        length: 550,
        width: 450,
        height: 350,
        maxWeight: 120,
        cost: 1_375_000m
    )
};


//var bins = new List<BinType>
//{
//    new() { Description = "Size 0", Length = 20, Width = 20, Height = 20, CostFunc = () => 50800 },
//    new() { Description = "Size 0", Length = 100, Width = 100, Height = 100, CostFunc = () => 60000 },

//};
//List<Item> items =
//[
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//    new(10,10,10),
//];

//var startTime = DateTime.Now;
//var chromosome = app.Execute(items, bins);
//var executionTime = (DateTime.Now - startTime).TotalSeconds;

//var packingResults = chromosome.PackingResults;
//Console.WriteLine(packingResults);
//Console.WriteLine("----------------------");

