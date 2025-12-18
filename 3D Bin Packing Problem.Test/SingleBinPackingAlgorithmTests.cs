using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Test;
/// <summary>
/// Exercises the single bin packing algorithm across placement and leftover item scenarios.
/// </summary>
public class SingleBinPackingAlgorithmTests
{
    [Fact]
    public void Execute_ShouldPackItem_WhenFeasible()
    {
        // Arrange
        var item = new Item(2, 2, 2);
        var binType = new BinType("Default", 10, 10, 10);

        var expectedPlacement = new PlacementResult(
            item,
            binType,
            new Vector3(0, 0, 0),
            new Vector3(2, 2, 2),
            1.0,
            0.75
        );

        var feasibilityChecker = new PlacementFeasibilityChecker();


        var subBinUpdatingAlgorithm = new SubBinUpdatingAlgorithm();
        var subBinOrderStrategyFactory = new SubBinOrderingStrategyFactory();

        var algorithm = new SingleBinPackingAlgorithm(
            feasibilityChecker,
            subBinUpdatingAlgorithm,
            subBinOrderStrategyFactory
        );


        // Act
        var result = algorithm.Execute(new List<Item> { item }, new BinInstance(binType));

        // Assert
        Assert.Single(result.PackedItems); // فقط یک آیتم باید بسته‌بندی شود
        Assert.Empty(result.LeftItems);    // نباید آیتمی باقی بماند

        var packed = result.PackedItems.First();
        Assert.Equal(item.Id, packed.ItemId);
        Assert.Equal(0, packed.Position.X);
        Assert.Equal(0, packed.Position.Y);
        Assert.Equal(0, packed.Position.Z);
        Assert.Equal(2, packed.Length);
        Assert.Equal(2, packed.Width);
        Assert.Equal(2, packed.Height);
        Assert.Equal(1, packed.SupportRatio);
    }

    // تست: آیتم بزرگ‌تر از Bin است → باید در LeftItems قرار گیرد
    [Fact]
    public void Execute_ShouldLeaveItem_WhenTooLarge()
    {
        // Arrange
        var item = new Item(20, 20, 20); // بزرگ‌تر از Bin
        var binType = new BinType("Default", 10, 10, 10);

        var feasibilityChecker = new PlacementFeasibilityChecker();


        var subBinUpdatingAlgorithm = new SubBinUpdatingAlgorithm();
        var subBinOrderStrategyFactory = new SubBinOrderingStrategyFactory();

        var algorithm = new SingleBinPackingAlgorithm(feasibilityChecker, subBinUpdatingAlgorithm, subBinOrderStrategyFactory);

        // Act
        var result = algorithm.Execute(new List<Item> { item }, new BinInstance(binType));

        // Assert
        Assert.Empty(result.PackedItems);
        Assert.Single(result.LeftItems);
    }

    // تست: بعضی آیتم‌ها جا می‌شوند و بعضی نمی‌شوند
    [Fact]
    public void Execute_ShouldPackSomeItems_AndLeaveOthers()
    {
        // Arrange
        var items = new List<Item>
    {
        new Item(5, 5, 5),   // ✅ جا میشه
        new Item(20, 20, 20) // ❌ جا نمیشه
    };

        var binType = new BinType("Default", 10, 10, 10);

        var placementResult = new PlacementResult(
            items[0],
            binType,
            new Vector3(0, 0, 0),
            new Vector3(5, 5, 5),
            1.0,
            0.75
        );


        var feasibilityChecker = new PlacementFeasibilityChecker();


        var subBinUpdatingAlgorithm = new SubBinUpdatingAlgorithm();
        var subBinOrderStrategyFactory = new SubBinOrderingStrategyFactory();


        var algorithm = new SingleBinPackingAlgorithm(
            feasibilityChecker,
            subBinUpdatingAlgorithm,
            subBinOrderStrategyFactory
        );

        // Act
        var result = algorithm.Execute(items, new BinInstance(binType));

        // Assert
        Assert.Single(result.PackedItems); // فقط یکی باید بسته‌بندی شود
        Assert.Single(result.LeftItems);   // یکی هم باید جا نشود

        var packed = result.PackedItems.First();
        Assert.Equal(items[0].Id, packed.ItemId);
        Assert.Equal(5, packed.Length);
        Assert.Equal(5, packed.Width);
        Assert.Equal(5, packed.Height);
    }

    // تست: بررسی ApplySpeedUpStrategy
    [Fact]
    public void ApplySpeedUpStrategy_ShouldFilterInvalidSubBins()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item(3, 3, 3),
            new Item(5, 5, 5)
        };

        var validSubBin = new SubBin(0, 0, 0, 10, 10, 10, 0, 0, 0, 0);
        var tooSmallSubBin = new SubBin(0, 0, 0, 2, 2, 2, 0, 0, 0, 0); // نباید انتخاب بشه

        var subBins = new List<SubBin> { validSubBin, tooSmallSubBin };

        var feasibilityChecker = new PlacementFeasibilityChecker();


        var subBinUpdatingAlgorithm = new SubBinUpdatingAlgorithm();
        var subBinOrderStrategyFactory = new SubBinOrderingStrategyFactory();

        var algorithm = new SingleBinPackingAlgorithm(feasibilityChecker, subBinUpdatingAlgorithm, subBinOrderStrategyFactory);

        // Act (استفاده از متد Private با Reflection یا تغییر دسترسی)
        var method = typeof(SingleBinPackingAlgorithm)
            .GetMethod("ApplySpeedUpStrategy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        var result = method?.Invoke(algorithm, [subBins, items]) as List<SubBin>;

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); // فقط یک sub-bin معتبر باید باقی بماند
        Assert.Equal(validSubBin, result[0]);
    }
    [Fact]
    public void Execute_ShouldPackEightSmallItemsIntoBin_WhenFeasible()
    {
        // Arrange
        var items = Enumerable.Range(0, 8)
            .Select(_ => new Item(1, 1, 1))
            .ToList();

        var binType = new BinType("Default", 2, 2, 2);


        var feasibilityChecker = new PlacementFeasibilityChecker();


        var subBinUpdatingAlgorithm = new SubBinUpdatingAlgorithm();
        var subBinOrderStrategyFactory = new SubBinOrderingStrategyFactory();

        var algorithm = new SingleBinPackingAlgorithm(
            feasibilityChecker,
            subBinUpdatingAlgorithm,
            subBinOrderStrategyFactory
        );

        // Act
        var result = algorithm.Execute(items, new BinInstance(binType));

        // Assert
        Assert.Equal(8, result.PackedItems.Count); // همه ۸ آیتم باید بسته شوند
        Assert.Empty(result.LeftItems);            // آیتمی نباید باقی بماند

        // بررسی اینکه همه نقاط پوشش داده شدند
        var cords = result.PackedItems.Select(p => (p.Position.X, p.Position.Y, p.Position.Z)).ToHashSet();
        Assert.Equal(8, cords.Count);
        Assert.Contains((0, 0, 0), cords);
        Assert.Contains((1, 0, 0), cords);
        Assert.Contains((0, 1, 0), cords);
        Assert.Contains((1, 1, 0), cords);
        Assert.Contains((0, 0, 1), cords);
        Assert.Contains((1, 0, 1), cords);
        Assert.Contains((0, 1, 1), cords);
        Assert.Contains((1, 1, 1), cords);
    }

}