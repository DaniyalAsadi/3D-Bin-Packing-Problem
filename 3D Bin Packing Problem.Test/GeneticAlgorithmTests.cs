using _3D_Bin_Packing_Problem.Core;
using _3D_Bin_Packing_Problem.Core.Model;
using FluentAssertions;
using System.Diagnostics;

namespace _3D_Bin_Packing_Problem.Test;

public class GeneticAlgorithmTests
{
    [Fact]
    public void Execute_Should_Return_Valid_Chromosome()
    {
        // Arrange
        var ga = GeneticAlgorithm.Default();

        var bins = new List<BinType>
        {
            BinType.Create("Small", 100, 100, 100, 1000, 10),
            BinType.Create("Medium", 200, 200, 200, 2000, 20),
        };

        var items = new List<Item>
        {
            Item.Create(50, 50, 50, 1, Guid.NewGuid()),
            Item.Create(80, 80, 80, 2, Guid.NewGuid())
        };

        // Act
        var result = ga.Execute(bins, items);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Fitness > double.MinValue);
    }
    [Fact]
    public void Execute_Should_Terminate()
    {
        var ga = GeneticAlgorithm.Default();

        var bins = new List<BinType>
        {
            BinType.Create("OnlyBin", 100, 100, 100, 1000, 10),
        };

        var items = new List<Item>
        {
            Item.Create(90, 90, 90, 1, Guid.NewGuid())
        };

        var sw = Stopwatch.StartNew();

        var result = ga.Execute(bins, items);

        sw.Stop();

        var assert = sw.Elapsed < TimeSpan.FromSeconds(20);
        assert.Should().BeTrue();
    }

}
