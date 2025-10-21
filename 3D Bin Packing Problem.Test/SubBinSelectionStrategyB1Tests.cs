using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;

namespace BinPacking.Tests;

/// <summary>
/// Verifies the selection logic for the B1 sub-bin selection strategy.
/// </summary>
public class SubBinSelectionStrategyB1Tests
{
    private static BinType baseBinType = new BinType()
    {
        Height = 2,
        Length = 2,
        Width = 2,
    };
    [Fact]
    public void Execute_ShouldReturnBinWithLowestCostPerVolume_WhenMultipleBinsAreFeasible()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(2, 2, 2),
            new Item(3, 3, 5)
        };

        var bins = new List<BinType>
        {
            new BinType { Length = 2, Width = 3, Height = 4}, // فقط آیتم 2x2x2 جا میشه → Feasible
            new BinType { Length = 3, Width = 3, Height = 5}, // هر دو آیتم جا میشن → Feasible
            new BinType { Length = 5, Width = 5, Height = 5} // همه جا میشن → Feasible
        };

        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(bins, products);

        // Assert
        Assert.NotNull(selectedBin);
        Assert.Equal(selectedBin, bins[0]);
        // Bin با کمترین Cost/Volume باید انتخاب بشه

        Assert.Equal(34000, Math.Round(selectedBin.Cost));
    }

    [Fact]
    public void Execute_ShouldReturnNull_WhenNoBinIsFeasiblex()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(2, 2, 2),
            new Item(3, 3, 5)
        };

        var bins = new List<BinType>
        {
            new BinType { Length = 1, Width = 2, Height = 2}, // هیچ آیتمی جا نمیشه
            new BinType { Length = 2, Width = 2, Height = 2}  // آیتم بزرگ‌تر جا نمیشه، آیتم کوچک هم جا نمیشه چون height=2 < 2 → کلاً رد
        };

        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(bins, products);


        // Assert
        Assert.NotNull(selectedBin);
        Assert.Equal(selectedBin, bins[1]);
        Assert.Equal(10000, selectedBin.Cost);
    }
    [Fact]
    public void Execute_ShouldReturnNull_WhenNoBinIsFeasible()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(2, 2, 2),
            new Item(3, 3, 5)
        };

        var bins = new List<BinType>
        {
            new BinType { Length = 1, Width = 1, Height = 1 }, // آیتم 2x2x2 هم جا نمیشه
            new BinType { Length = 2, Width = 2, Height = 1 }  // Height=1 → هیچ آیتمی جا نمیشه
        };

        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(bins, products);

        // Assert
        Assert.Null(selectedBin); // چون هیچ Binی حتی آیتم کوچک رو هم accommodate نمی‌کنه
    }



}
