using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using FluentAssertions;
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
        var item = new Item(new Dimensions(2, 2, 2));
        var binType = new BinType("Default", new Dimensions(10, 10, 10));

        var expectedPlacement = new PlacementResult(
            item,
            binType,
            new Vector3(0, 0, 0),
            new Dimensions(2, 2, 2),
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
        result.PackedItems.Should().ContainSingle();
        result.LeftItems.Should().BeEmpty();

        var packed = result.PackedItems.Should().ContainSingle().Which;

        packed.ItemId.Should().Be(item.Id);
        packed.Position.X.Should().Be(0);
        packed.Position.Y.Should().Be(0);
        packed.Position.Z.Should().Be(0);
        packed.Length.Should().Be(2);
        packed.Width.Should().Be(2);
        packed.Height.Should().Be(2);
        packed.SupportRatio.Should().Be(1);

    }

    // تست: آیتم بزرگ‌تر از Bin است → باید در LeftItems قرار گیرد
    [Fact]
    public void Execute_ShouldLeaveItem_WhenTooLarge()
    {
        // Arrange
        var item = new Item(new Dimensions(20, 20, 20)); // بزرگ‌تر از Bin
        var binType = new BinType("Default", new Dimensions(10, 10, 10));

        var feasibilityChecker = new PlacementFeasibilityChecker();


        var subBinUpdatingAlgorithm = new SubBinUpdatingAlgorithm();
        var subBinOrderStrategyFactory = new SubBinOrderingStrategyFactory();

        var algorithm = new SingleBinPackingAlgorithm(feasibilityChecker, subBinUpdatingAlgorithm, subBinOrderStrategyFactory);

        // Act
        var result = algorithm.Execute(new List<Item> { item }, new BinInstance(binType));

        // Assert
        result.PackedItems.Should().BeEmpty();
        result.LeftItems.Should().ContainSingle();
    }

    // تست: بعضی آیتم‌ها جا می‌شوند و بعضی نمی‌شوند
    [Fact]
    public void Execute_ShouldPackSomeItems_AndLeaveOthers()
    {
        // Arrange
        var items = new List<Item>
    {
        new Item(new Dimensions(5, 5, 5)),   // ✅ جا میشه
        new Item(new Dimensions(20, 20, 20)) // ❌ جا نمیشه
    };

        var binType = new BinType("Default", new Dimensions(10, 10, 10));

        var placementResult = new PlacementResult(
            items[0],
            binType,
            new Vector3(0, 0, 0),
            new Dimensions(5, 5, 5),
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
        result.PackedItems.Should().ContainSingle(); // فقط یکی باید بسته‌بندی شود
        result.LeftItems.Should().ContainSingle();   // یکی هم باید جا نشود

        var packed = result.PackedItems.First();
        packed.ItemId.Should().Be(items[0].Id);
        packed.Length.Should().Be(5);
        packed.Width.Should().Be(5);
        packed.Height.Should().Be(5);
    }

    // تست: بررسی ApplySpeedUpStrategy
    [Fact]
    public void ApplySpeedUpStrategy_ShouldFilterInvalidSubBins()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item(new Dimensions(3, 3, 3)),
            new Item(new Dimensions(5, 5, 5))
        };

        var validSubBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(10, 10, 10));
        var tooSmallSubBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(2, 2, 2)); // نباید انتخاب بشه

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
        result.Should().NotBeNull();
        result.Should().ContainSingle(); // فقط یک sub-bin معتبر باید باقی بماند
        result.Should().HaveElementAt(0, validSubBin);
    }
    [Fact]
    public void Execute_ShouldPackEightSmallItemsIntoBin_WhenFeasible()
    {
        // Arrange
        var items = Enumerable.Range(0, 8)
            .Select(_ => new Item(new Dimensions(1, 1, 1)))
            .ToList();

        var binType = new BinType("Default", new Dimensions(2, 2, 2));


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
        result.PackedItems.Should().HaveCount(8);
        result.LeftItems.Should().BeEmpty();

        // بررسی اینکه همه نقاط پوشش داده شدند
        var coords = result.PackedItems
            .Select(p => (p.Position.X, p.Position.Y, p.Position.Z))
            .ToHashSet();

        coords.Should().HaveCount(8);
        coords.Should().Contain((0, 0, 0));
        coords.Should().Contain((1, 0, 0));
        coords.Should().Contain((0, 1, 0));
        coords.Should().Contain((1, 1, 0));
        coords.Should().Contain((0, 0, 1));
        coords.Should().Contain((1, 0, 1));
        coords.Should().Contain((0, 1, 1));
        coords.Should().Contain((1, 1, 1));

    }

}