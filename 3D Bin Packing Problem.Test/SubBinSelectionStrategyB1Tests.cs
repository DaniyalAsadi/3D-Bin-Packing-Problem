using _3D_Bin_Packing_Problem.Core.Datasets;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;
using FluentAssertions;

namespace _3D_Bin_Packing_Problem.Test;
/// <summary>
/// Verifies the selection logic for the B1 sub-bin selection strategy.
/// </summary>
public class SubBinSelectionStrategyB1Tests
{
    private readonly List<BinType> _presetBinTypes = BinTypeDataset.StandardBinTypes();

    [Fact]
    public void Execute_ShouldReturnBinWithLowestCostPerVolume_WhenMultipleBinsAreFeasible()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(new Dimensions(200, 200, 200)),
            new Item(new Dimensions(300, 300, 500))
        };
        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(_presetBinTypes, products);

        // Assert
        selectedBin.Should().NotBeNull();
        _presetBinTypes.Should().HaveElementAt(4, selectedBin); // سایز ۵ انتظار می‌رود
    }

    [Fact]
    public void Execute_ShouldReturnBestFittingBin_WhenItemsPartiallyFit()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(new Dimensions(150, 100, 150)),
        };
        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(_presetBinTypes, products);

        // Assert
        selectedBin.Should().NotBeNull();
        _presetBinTypes.Should().HaveElementAt(2, selectedBin); // سایز ۳ بهترین گزینه
    }

    [Fact]
    public void Execute_ShouldReturnSmallestFeasibleBin_WhenItemIsSmall()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(new Dimensions(150, 100, 100)),
        };
        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(_presetBinTypes, products);

        // Assert
        selectedBin.Should().NotBeNull();
        _presetBinTypes.Should().HaveElementAt(0, selectedBin); // سایز ۱ کافی است
    }

    [Fact]
    public void Execute_ShouldReturnNull_WhenNoBinCanAccommodateAnyItem()
    {
        // Arrange
        var products = new List<Item>
        {
            new Item(new Dimensions(2, 2, 2)),
            new Item(new Dimensions(3, 3, 5))
        };

        var bins = new List<BinType>
        {
            new BinType("Default",new Dimensions(1,1,1)),
            new BinType ("Default",new Dimensions(2,2,1))
        };

        var strategy = new SubBinSelectionStrategyB1();

        // Act
        var selectedBin = strategy.Execute(bins, products);

        // Assert
        selectedBin.Should().BeNull();
    }
}
