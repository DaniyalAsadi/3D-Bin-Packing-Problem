namespace _3D_Bin_Packing_Problem.Test;
using _3D_Bin_Packing_Problem.Core.Model;
using FluentAssertions;
using System;
using Xunit;

public class BinTypeTests
{
    [Fact]
    public void Create_Should_Create_Valid_BinType()
    {
        // Act
        var bin = BinType.Create(
            name: "Size 1",
            length: 100,
            width: 80,
            height: 60,
            maxWeight: 50,
            cost: 120_000m,
            tareWeight: 10
        );

        // Assert
        bin.Should().NotBeNull();
        bin.Id.Should().NotBeEmpty();
        bin.Name.Should().Be("Size 1");

        bin.InnerDimensions.Length.Should().Be(100);
        bin.InnerDimensions.Width.Should().Be(80);
        bin.InnerDimensions.Height.Should().Be(60);

        bin.MaxWeight.Should().Be(50);
        bin.Cost.Should().Be(120_000m);
        bin.TareWeight.Should().Be(10);
    }

    [Fact]
    public void Volume_Should_Be_Calculated_Correctly()
    {
        // Act
        var bin = BinType.Create(
            name: "Test",
            length: 10,
            width: 5,
            height: 2,
            maxWeight: 10,
            cost: 100
        );

        // Assert
        bin.Volume.Should().Be(100);
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Null_Or_Whitespace()
    {
        // Act
        Action act = () => BinType.Create(
            name: " ",
            length: 10,
            width: 10,
            height: 10,
            maxWeight: 10,
            cost: 100
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0, 10, 10)]
    [InlineData(10, 0, 10)]
    [InlineData(10, 10, 0)]
    [InlineData(-1, 10, 10)]
    public void Create_Should_Throw_When_Dimension_Is_Invalid(
        float length, float width, float height)
    {
        // Act
        Action act = () => BinType.Create(
            name: "Invalid",
            length: length,
            width: width,
            height: height,
            maxWeight: 10,
            cost: 100
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Throw_When_MaxWeight_Is_Invalid()
    {
        // Act
        Action act = () => BinType.Create(
            name: "Invalid",
            length: 10,
            width: 10,
            height: 10,
            maxWeight: 0,
            cost: 100
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Throw_When_Cost_Is_Negative()
    {
        // Act
        Action act = () => BinType.Create(
            name: "Invalid",
            length: 10,
            width: 10,
            height: 10,
            maxWeight: 10,
            cost: -1
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Throw_When_TareWeight_Is_Negative()
    {
        // Act
        Action act = () => BinType.Create(
            name: "Invalid",
            length: 10,
            width: 10,
            height: 10,
            maxWeight: 10,
            cost: 100,
            tareWeight: -5
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Clone_Should_Create_Deep_Copy_With_Same_Values()
    {
        // Arrange
        var original = BinType.Create(
            name: "CloneMe",
            length: 100,
            width: 50,
            height: 40,
            maxWeight: 80,
            cost: 200_000m,
            tareWeight: 15
        );

        // Act
        var clone = original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Id.Should().NotBe(original.Id);

        clone.Name.Should().Be(original.Name);
        clone.InnerDimensions.Length.Should().Be(original.InnerDimensions.Length);
        clone.InnerDimensions.Width.Should().Be(original.InnerDimensions.Width);
        clone.InnerDimensions.Height.Should().Be(original.InnerDimensions.Height);

        clone.MaxWeight.Should().Be(original.MaxWeight);
        clone.Cost.Should().Be(original.Cost);
        clone.TareWeight.Should().Be(original.TareWeight);
    }

    [Fact]
    public void Implicit_Conversion_To_SubBin_Should_Work()
    {
        // Arrange
        var bin = BinType.Create(
            name: "Implicit",
            length: 100,
            width: 100,
            height: 100,
            maxWeight: 100,
            cost: 300_000m
        );

        // Act
        SubBin subBin = bin;

        // Assert
        subBin.Should().NotBeNull();
    }

    [Fact]
    public void ToString_Should_Return_Readable_Representation()
    {
        // Act
        var bin = BinType.Create(
            name: "Readable",
            length: 10,
            width: 20,
            height: 30,
            maxWeight: 50,
            cost: 99.99m
        );

        // Assert
        bin.ToString()
           .Should().Contain("Readable")
           .And.Contain("L×W×H")
           .And.Contain("Cost");
    }
}
