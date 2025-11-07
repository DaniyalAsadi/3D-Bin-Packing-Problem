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

// ------------------------
//// تنظیمات سرویس‌ها
//var services = new ServiceCollection();
//SettingsManager.Initialize(configure: settings =>
//{
//    // Genetic Algorithm Settings
//    settings.Genetic.PopulationSize = 50;
//    settings.Genetic.MaxIteration = 100;
//    settings.Genetic.MutationRate = 0.05;
//    settings.Genetic.CrossoverRate = 0.85;
//    settings.Genetic.TournamentGroupSize = 5;
//    settings.Genetic.ElitismPopulationSize = 5;
//    settings.Genetic.SupportThreshold = 0.75;
//    settings.Genetic.AlphaWeight = 1.0;
//    settings.Genetic.BetaWeight = 1.0;
//    settings.Genetic.PenaltyCoefficient = 200000;

//    // Packing Settings
//    settings.ItemOrdering = ItemOrderingStrategyType.I1;
//    settings.SubBinOrdering = SubBinOrderingStrategyType.S1;
//    settings.SubBinSelection = SubBinSelectionStrategyType.B1;
//    settings.Crossover = CrossoverType.All;
//    settings.Mutation = MutationType.All;
//});

//services.AddLogging(builder =>
//{
//    builder.AddConsole();
//    builder.SetMinimumLevel(LogLevel.Debug);
//});

//// ثبت سرویس‌ها
//services.AddScoped<GeneticAlgorithm>();
//services.AddScoped<IPopulationGenerator, PopulationGenerator>();
//services.AddScoped<ISelection, RouletteWheelSelection>();
//services.AddScoped<IFitnessCalculator, DefaultFitnessCalculator>();

//services.AddSingleton<ItemOrderingStrategyFactory>();
//services.AddSingleton<SubBinOrderingStrategyFactory>();
//services.AddSingleton<SubBinSelectionStrategyFactory>();

//services.AddSingleton<CrossoverFactory>();
//services.AddSingleton<MutationFactory>();

//services.AddScoped<IComparer<Chromosome>, ChromosomeFitnessComparer>();

//services.AddScoped<IPlacementAlgorithm, PlacementAlgorithm>();
//services.AddScoped<ISingleBinPackingAlgorithm, SingleBinPackingAlgorithm>();
//services.AddScoped<IPlacementFeasibilityChecker, PlacementFeasibilityChecker>();
//services.AddScoped<ISubBinUpdatingAlgorithm, SubBinUpdatingAlgorithm>();

//var serviceProvider = services.BuildServiceProvider();

//// تولید بنچمارک کامل
//Console.WriteLine("Generating complete benchmark suite...");
//var benchmarkSuite = BenchmarkGenerator.GenerateCompleteBenchmark();
//benchmarkSuite.PrintSummary();

//// اجرای تست‌ها بر روی نمونه‌های مختلف
//var geneticAlgorithm = serviceProvider.GetRequiredService<GeneticAlgorithm>();

//// تست روی نمونه‌های مختلف از کلاس‌های مختلف
//var testInstances = new[]
//{
//    benchmarkSuite.GetInstance("Class_1_50_1"),
//    benchmarkSuite.GetInstance("Class_2_100_1"),
//    benchmarkSuite.GetInstance("Class_3_150_1"),
//    benchmarkSuite.GetInstance("Class_6_50_1"),
//    benchmarkSuite.GetInstance("Class_8_100_1")
//}.Where(instance => instance != null).ToList();

//Console.WriteLine("\n=== Running Genetic Algorithm on Benchmark Instances ===");

//foreach (var instance in testInstances)
//{
//    Console.WriteLine($"\n--- Testing Instance: {instance.InstanceName} ---");
//    Console.WriteLine($"Items: {instance.Items.Count}, Available Bins: {instance.Bins.Count}");

//    try
//    {
//        var result = geneticAlgorithm.Execute(instance.Bins, instance.Items);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"✓ Success:\n{result.PackingResults}");
//        Console.ForegroundColor = ConsoleColor.Gray;

//        // نمایش خلاصه نتایج
//        PrintResultSummary(instance, result);
//    }
//    catch (Exception ex)
//    {
//        Console.ForegroundColor = ConsoleColor.Red;
//        Console.WriteLine($"✗ Error: {ex.Message}");
//        Console.ForegroundColor = ConsoleColor.Gray;
//    }
//}

//// تست استراتژی‌های مختلف
//Console.WriteLine("\n=== Testing Different Strategy Combinations ===");
//TestStrategyCombinations(geneticAlgorithm, benchmarkSuite);

//void PrintResultSummary(BenchmarkInstance instance, Chromosome result)
//{
//    Console.WriteLine($"Instance: {instance.InstanceName}");
//    Console.WriteLine($"Total Items: {instance.Items.Count}");
//    Console.WriteLine($"Total Item Volume: {instance.Items.Sum(i => i.Volume)}");
//    Console.WriteLine($"Used Bins: {result.PackingResults?.UsedBinTypes.Count ?? 0}");
//    Console.WriteLine($"Total Cost: {result.PackingResults?.UsedBinTypes.Sum(e => e.BinType.Cost) ?? 0:F2}");
//    Console.WriteLine($"Fitness: {result.Fitness:F2}");
//    Console.WriteLine("---");
//}

//void TestStrategyCombinations(GeneticAlgorithm ga, BenchmarkSuite suite)
//{
//    var testInstance = suite.GetInstance("Class_1_50_1");
//    if (testInstance == null) return;

//    // تست ترکیب‌های مختلف استراتژی
//    var strategies = new[]
//    {
//        (ItemOrderingStrategyType.I1, SubBinOrderingStrategyType.S1, SubBinSelectionStrategyType.B1),
//        (ItemOrderingStrategyType.I2, SubBinOrderingStrategyType.S2, SubBinSelectionStrategyType.B2),
//        (ItemOrderingStrategyType.I3, SubBinOrderingStrategyType.S3, SubBinSelectionStrategyType.B3),
//        (ItemOrderingStrategyType.I1, SubBinOrderingStrategyType.S5, SubBinSelectionStrategyType.B1), // SC1
//        (ItemOrderingStrategyType.I3, SubBinOrderingStrategyType.S5, SubBinSelectionStrategyType.B3), // SC2
//        (ItemOrderingStrategyType.I3, SubBinOrderingStrategyType.S2, SubBinSelectionStrategyType.B2), // SC8
//    };

//    foreach (var (itemStrategy, subBinStrategy, binStrategy) in strategies)
//    {
//        Console.WriteLine($"\nTesting Strategy: I{(int)itemStrategy + 1}, S{(int)subBinStrategy + 1}, B{(int)binStrategy + 1}");

//        // تغییر تنظیمات استراتژی
//        SettingsManager.Update(settings =>
//        {
//            settings.ItemOrdering = itemStrategy;
//            settings.SubBinOrdering = subBinStrategy;
//            settings.SubBinSelection = binStrategy;
//        });

//        try
//        {
//            var result = ga.Execute(testInstance.Bins, testInstance.Items);
//            Console.WriteLine($"  Result: Cost = {result.PackingResults?.TotalCost ?? 0:F2}, " +
//                            $"Fitness = {result.Fitness:F2}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"  Error: {ex.Message}");
//        }
//    }
//}
//public static class ResultPrinter
//{
//    public static void PrintResultSummary(BenchmarkInstance instance, Chromosome result)
//    {
//        Console.ForegroundColor = ConsoleColor.Cyan;
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║         BENCHMARK RESULTS           ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");
//        Console.ResetColor();

//        // Instance Information
//        Console.WriteLine($"🏷️  INSTANCE: {instance.InstanceName}");
//        Console.WriteLine($"📋 TOTAL ITEMS: {instance.Items.Count}");
//        Console.WriteLine($"📦 TOTAL ITEM VOLUME: {instance.Items.Sum(i => i.Volume):N0}");

//        // Results
//        if (result?.PackingResults != null)
//        {
//            var packingResults = result.PackingResults;

//            Console.WriteLine($"✅ USED BINS: {packingResults.UsedBinTypes.Count}");
//            Console.WriteLine($"💰 TOTAL COST: {packingResults.UsedBinTypes.Sum(e => e.BinType.Cost):F2}");
//            Console.WriteLine($"🎯 FITNESS: {result.Fitness:F2}");

//            // Additional metrics
//            Console.WriteLine($"📊 PACKED ITEMS: {packingResults.PackedItems.Count}");
//            Console.WriteLine($"⚠️  LEFT ITEMS: {packingResults.LeftItems.Count}");
//            Console.WriteLine($"📈 UTILIZATION: {CalculateUtilization(packingResults):P2}");
//        }
//        else
//        {
//            Console.ForegroundColor = ConsoleColor.Yellow;
//            Console.WriteLine("⚠️  No packing results available");
//            Console.ResetColor();
//        }

//        Console.ForegroundColor = ConsoleColor.DarkGray;
//        Console.WriteLine("──────────────────────────────────────");
//        Console.ResetColor();
//    }

//    public static void PrintDetailedComparison(List<BenchmarkResult> results)
//    {
//        Console.ForegroundColor = ConsoleColor.Magenta;
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║        COMPARISON RESULTS           ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");
//        Console.ResetColor();

//        Console.WriteLine("┌─────────────┬──────────┬──────────┬──────────┬──────────┐");
//        Console.WriteLine("│ Instance    │   Cost   │ Fitness  │  Bins    │ Utilization │");
//        Console.WriteLine("├─────────────┼──────────┼──────────┼──────────┼──────────┤");

//        foreach (var result in results)
//        {
//            Console.WriteLine($"│ {result.InstanceName,-11} │ {result.TotalCost,8:F2} │ {result.Fitness,8:F2} │ {result.UsedBins,8} │ {result.Utilization,8:P2} │");
//        }

//        Console.WriteLine("└─────────────┴──────────┴──────────┴──────────┴──────────┘");
//    }

//    private static double CalculateUtilization(PackingResultsViewModel packingResults)
//    {
//        if (packingResults.UsedBinTypes.Count == 0) return 0;

//        double totalBinVolume = packingResults.UsedBinTypes.Sum(bin => bin.BinType.Volume);
//        double totalPackedVolume = packingResults.PackedItems.Sum(item => item.Volume);

//        return totalBinVolume > 0 ? totalPackedVolume / totalBinVolume : 0;
//    }
//}

//// کلاس کمکی برای ذخیره نتایج مقایسه
//public class BenchmarkResult
//{
//    public string InstanceName { get; set; }
//    public double TotalCost { get; set; }
//    public double Fitness { get; set; }
//    public int UsedBins { get; set; }
//    public double Utilization { get; set; }
//}
