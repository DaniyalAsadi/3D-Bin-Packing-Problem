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

namespace _3D_Bin_Packing_Problem;


public static class Program
{
    public static void Main(string[] args)
    {
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
            new(2, 2, 2 ),
            new(3, 3 ,5),
        ];
        var x = app.Execute(products);


        Console.WriteLine(x.PackingResults);


        //Test.Execute(args);
        //List<Product> products =
        //[
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //    new() { Id = Guid.NewGuid(), Length = 2, Width = 2, Height = 2 },
        //];
        //List<Box> boxes =
        //[
        //    new() { Id = Guid.NewGuid(), Length = 4, Width = 4, Height = 4 }
        //];
        ////var geneticAlgorithm = new GeneticAlgorithm(products.OrderBy(e => e.Id).ToList(), boxes);
        ////var x = geneticAlgorithm.Execute();
        ////Console.WriteLine(x);
        //var boxPlacement = new BoxPlacement(boxes[0]);
        //List<ProductPlacement> productPlacements =
        //[
        //    new (products[0], new Vector3(1, 1, 1)),
        //    new (products[1], new Vector3(3, 1, 1)),
        //    new (products[2], new Vector3(1, 3, 1)),
        //    new (products[3], new Vector3(3, 3, 1)),
        //    new (products[4], new Vector3(1, 1, 3)),
        //    new (products[5], new Vector3(3, 1, 3)),
        //    new (products[6], new Vector3(1, 3, 3)),
        //    new (products[7], new Vector3(3, 3, 3)),
        //];
        //boxPlacement.PlaceProduct(productPlacements);

        //var chromosome = new Chromosome(boxPlacement);
        //Console.WriteLine(chromosome);
        //Console.WriteLine(chromosome.Fitness);



        //var m = FitnessCalculator.OverLappingMatrix(chromosome);
        //for (var i = 0; i < m.GetLength(0); i++)
        //{
        //    for (int j = 0; j < m.GetLength(1); j++)
        //    {
        //        if (m[i, j])
        //        {
        //            Console.Write(1);
        //        }
        //        else
        //        {
        //            Console.Write(0);
        //        }
        //    }
        //    Console.WriteLine();

        //}



        //List<Box> boxes = new()
        //{
        //    new() { Id = 1, Length = 15, Width = 10, Height = 10, Price = 3600},
        //    new() { Id = 2, Length = 20, Width = 15, Height = 10, Price = 6450},
        //    new() { Id = 3, Length = 20, Width = 20, Height = 15, Price = 8900},
        //    new() { Id = 4, Length = 30, Width = 20, Height = 20, Price = 12000},
        //    new() { Id = 5, Length = 35, Width = 25, Height = 20, Price = 16300},
        //    new() { Id = 6, Length = 45, Width = 25, Height = 20, Price = 18900},
        //    new() { Id = 7, Length = 40, Width = 30, Height = 25, Price = 22400},
        //    new() { Id = 8, Length = 45, Width = 40, Height = 30, Price = 55000},
        //    new() { Id = 9, Length = 55, Width = 45, Height = 35, Price = 80148},
        //};

        //chromosome.BoxPlacements.ToList().ForEach(Console.WriteLine);


        Item product21312 = new Item(7, 1, 4);
        Item product21313 = new Item(1, 7, 4);

    }
}
