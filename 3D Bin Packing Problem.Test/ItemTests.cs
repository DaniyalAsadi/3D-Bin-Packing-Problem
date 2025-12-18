using _3D_Bin_Packing_Problem.Core.Model;
using FluentAssertions;

namespace _3D_Bin_Packing_Problem.Test;
public class ItemTests
{
    [Fact]
    public void Create_Should_Create_Valid_Item()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var item = Item.Create(
            length: 10,
            width: 20,
            height: 30,
            weight: 5,
            orderId: orderId
        );

        // Assert
        item.Should().NotBeNull();
        item.Id.Should().NotBeEmpty();
        item.OrderId.Should().Be(orderId);

        item.Dimensions.Length.Should().Be(10);
        item.Dimensions.Width.Should().Be(20);
        item.Dimensions.Height.Should().Be(30);

        item.Weight.Should().Be(5);
        item.IsFragile.Should().BeFalse();
        item.IsStackable.Should().BeTrue();
        item.MaxLoadOnTop.Should().BeNull();
        item.LoadType.Should().Be(LoadType.Normal);
    }

    [Fact]
    public void Volume_Should_Be_Calculated_Correctly()
    {
        // Act
        var item = Item.Create(
            length: 2,
            width: 3,
            height: 4,
            weight: 1,
            orderId: Guid.NewGuid()
        );

        // Assert
        item.Volume.Should().Be(24);
    }

    [Fact]
    public void GetMinimumDimension_Should_Return_Smallest_Value()
    {
        // Act
        var item = Item.Create(
            length: 15,
            width: 5,
            height: 10,
            weight: 1,
            orderId: Guid.NewGuid()
        );

        // Assert
        item.GetMinimumDimension().Should().Be(5);
    }

    [Fact]
    public void Create_Should_Throw_When_Dimension_Is_Zero_Or_Negative()
    {
        // Act
        var act = () => Item.Create(
            length: 0,
            width: 10,
            height: 10,
            weight: 1,
            orderId: Guid.NewGuid()
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Throw_When_Weight_Is_Negative_Or_Zero()
    {
        // Act
        var act = () => Item.Create(
            length: 10,
            width: 10,
            height: 10,
            weight: 0,
            orderId: Guid.NewGuid()
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Throw_When_Item_Is_Fragile_And_Stackable()
    {
        // Act
        var act = () => Item.Create(
            length: 10,
            width: 10,
            height: 10,
            weight: 2,
            orderId: Guid.NewGuid(),
            isFragile: true,
            isStackable: true
        );

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Fragile items cannot be stackable*");
    }

    [Fact]
    public void Create_Should_Throw_When_MaxLoadOnTop_Is_Set_For_NonStackable_Item()
    {
        // Act
        var act = () => Item.Create(
            length: 10,
            width: 10,
            height: 10,
            weight: 2,
            orderId: Guid.NewGuid(),
            isStackable: false,
            maxLoadOnTop: 100
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Allow_Custom_Orientations()
    {
        // Arrange
        var orientations = new List<Orientation>
        {
            Orientation.Xy,
            Orientation.Zx
        };

        // Act
        var item = Item.Create(
            length: 10,
            width: 20,
            height: 30,
            weight: 3,
            orderId: Guid.NewGuid(),
            orientations: orientations
        );

        // Assert
        item.Orientations.Should().BeEquivalentTo(orientations);
    }

    [Fact]
    public void ToString_Should_Return_Readable_Dimensions()
    {
        // Act
        var item = Item.Create(
            length: 1,
            width: 2,
            height: 3,
            weight: 1,
            orderId: Guid.NewGuid()
        );

        // Assert
        item.ToString().Should().Contain("Length")
            .And.Contain("Width")
            .And.Contain("Height");
    }
    [Fact]
    public void Create_Should_Throw_When_MaxLoadOnTop_Is_Set_But_Item_Is_Not_Stackable()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        Action act = () => Item.Create(
            length: 10,
            width: 10,
            height: 10,
            weight: 2,
            orderId: orderId,
            isStackable: false,
            maxLoadOnTop: 50
        );

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*MaxLoadOnTop*");
    }

}
