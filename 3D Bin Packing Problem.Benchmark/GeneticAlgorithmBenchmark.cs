using _3D_Bin_Packing_Problem.Core;
using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Datasets;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace _3D_Bin_Packing_Problem.Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class GeneticAlgorithmBenchmark
{
    private GeneticAlgorithm _geneticAlgorithm;
    private List<BinType> _binTypes;
    private List<Item> _items;
    private readonly Guid _orderId = Guid.NewGuid();

    // Benchmark Parameters
    [ParamsAllValues]
    public ItemOrderingStrategyType ItemOrdering { get; set; }

    [ParamsAllValues]
    public SubBinOrderingStrategyType SubBinOrdering { get; set; }

    [ParamsAllValues]
    public SubBinSelectionStrategyType SubBinSelection { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _geneticAlgorithm = GeneticAlgorithm.Default();

        SettingsManager.Initialize(settings =>
        {
            // Genetic Algorithm Settings
            settings.Genetic.PopulationSize = 30;
            settings.Genetic.MaxIteration = 50;
            settings.Genetic.MutationRate = 0.05;
            settings.Genetic.CrossoverRate = 0.85;
            settings.Genetic.TournamentGroupSize = 5;
            settings.Genetic.ElitismPopulationSize = 5;
            settings.Genetic.SupportThreshold = 1;
            settings.Genetic.AlphaWeight = 1.0f;
            settings.Genetic.BetaWeight = 1.0f;
            settings.Genetic.PenaltyCoefficient = 20000;

            // Packing Settings — use the current benchmark scenario
            settings.ItemOrdering = ItemOrdering;
            settings.SubBinOrdering = SubBinOrdering;
            settings.SubBinSelection = SubBinSelection;
            settings.Crossover = CrossoverType.All;
            settings.Mutation = MutationType.All;
        });

        _binTypes = BinTypeDataset.StandardBinTypes();

        _items = new List<Item>
        {
            Item.Create(new Dimensions(10, 10, 10), 5, _orderId),
            Item.Create(new Dimensions(20, 10, 10), 8, _orderId),
            Item.Create(new Dimensions(30, 20, 15), 15, _orderId),
            Item.Create(new Dimensions(15, 15, 5), 4, _orderId),
            Item.Create(new Dimensions(8, 8, 20), 6, _orderId)
        };
    }

    [Benchmark]
    public void RunGeneticAlgorithm()
    {
        _geneticAlgorithm.Execute(_binTypes, _items);
    }
}
