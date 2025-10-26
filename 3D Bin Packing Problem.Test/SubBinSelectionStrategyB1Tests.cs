using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

namespace BinPacking.Tests;

/// <summary>
/// Verifies the selection logic for the B1 sub-bin selection strategy.
/// </summary>
public class SubBinSelectionStrategyB1Tests
{
    private readonly List<BinType> PresetBinTypes = new()
    {
        new BinType { Description = "سایز ۱", Length = 150, Width = 100, Height = 100, CostFunc = () => 63800 },
        new BinType { Description = "سایز ۲", Length = 200, Width = 150, Height = 100, CostFunc = () => 115500 },
        new BinType { Description = "سایز ۳", Length = 200, Width = 200, Height = 150, CostFunc = () => 172700 },
        new BinType { Description = "سایز ۴", Length = 300, Width = 200, Height = 200, CostFunc = () => 247500 },
        new BinType { Description = "سایز ۵", Length = 350, Width = 250, Height = 200, CostFunc = () => 446600 },
        new BinType { Description = "سایز ۶", Length = 450, Width = 250, Height = 200, CostFunc = () => 559900 },
        new BinType { Description = "سایز ۷", Length = 400, Width = 300, Height = 250, CostFunc = () => 686400 },
        new BinType { Description = "سایز ۸", Length = 450, Width = 400, Height = 300, CostFunc = () => 1043900 },
        new BinType { Description = "سایز ۹", Length = 550, Width = 450, Height = 350, CostFunc = () => 1375000 }
    };

    [Fact]
    public void Execute_ShouldReturnBinWithLowestCostPerVolume_WhenMultipleBinsAreFeasible()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(200, 200, 200),
            new Item(300, 300, 500)
        };
        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(PresetBinTypes, products);

        // Assert
        Assert.NotNull(selectedBin);
        Assert.Equal(PresetBinTypes[4], selectedBin); // سایز ۵ انتظار می‌رود
    }

    [Fact]
    public void Execute_ShouldReturnBestFittingBin_WhenItemsPartiallyFit()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(150, 100, 150),
        };
        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(PresetBinTypes, products);

        // Assert
        Assert.NotNull(selectedBin);
        Assert.Equal(PresetBinTypes[2], selectedBin); // سایز ۳ بهترین گزینه
    }

    [Fact]
    public void Execute_ShouldReturnSmallestFeasibleBin_WhenItemIsSmall()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(150, 100, 100),
        };
        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(PresetBinTypes, products);

        // Assert
        Assert.NotNull(selectedBin);
        Assert.Equal(PresetBinTypes[0], selectedBin); // سایز ۱ کافی است
    }

    [Fact]
    public void Execute_ShouldReturnNull_WhenNoBinCanAccommodateAnyItem()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(2, 2, 2),
            new Item(3, 3, 5)
        };

        var bins = new List<BinType>
        {
            new BinType { Length = 1, Width = 1, Height = 1 },
            new BinType { Length = 2, Width = 2, Height = 1 }
        };

        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(bins, products);

        // Assert
        Assert.Null(selectedBin);
    }
}
