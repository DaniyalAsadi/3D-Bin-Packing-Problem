using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.ViewModels;
using Moq;

/// <summary>
/// Validates the placement algorithm behavior under various bin availability scenarios.
/// </summary>
public class PlacementAlgorithmTests
{
    [Fact]
    public void Execute_ShouldPackAllItems_WhenBinIsSufficient()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item(5, 5, 5),
            new Item(4, 4, 4)
        };

        var binType = new BinType { Length = 10, Width = 10, Height = 10 };

        var itemOrdering = new Mock<IItemOrderingStrategy>();
        itemOrdering.Setup(x => x.Apply(It.IsAny<IEnumerable<Item>>()))
            .Returns((IEnumerable<Item> it) => it);

        var subBinSelection = new Mock<ISubBinSelectionStrategy>();
        subBinSelection.Setup(x => x.Execute(It.IsAny<IEnumerable<BinType>>(), It.IsAny<List<Item>>()))
            .Returns(binType);

        var singleBinPacking = new Mock<ISingleBinPackingAlgorithm>();
        singleBinPacking.Setup(x => x.Execute(It.IsAny<List<Item>>(), binType))
            .Returns(new PackingResultViewModel
            {
                PackedItems = items.Select(i => new PackedItemViewModel() { ItemId = i.Id }).ToList(),
                LeftItems = new List<ItemViewModel>()
            });

        var algorithm = new PlacementAlgorithm(itemOrdering.Object, subBinSelection.Object, singleBinPacking.Object);

        // Act
        var result = algorithm.Execute(items, new List<BinType> { binType });

        // Assert
        Assert.Equal(2, result.PackedItems.Count);
        Assert.Single(result.UsedBinTypes);
        Assert.Empty(result.LeftItems);
    }
    [Fact]
    public void Execute_ShouldPackSmallItem_AndLeaveTooLargeItem()
    {
        // Arrange
        var items = new List<Item>
    {
        new Item(5, 5, 5),   // جا می‌شود
        new Item(20, 20, 20) // بزرگ‌تر از Bin → جا نمی‌شود
    };

        var binType = new BinType { Length = 10, Width = 10, Height = 10 };

        var itemOrdering = new Mock<IItemOrderingStrategy>();
        itemOrdering.Setup(x => x.Apply(It.IsAny<IEnumerable<Item>>()))
            .Returns((IEnumerable<Item> it) => it);

        var subBinSelection = new Mock<ISubBinSelectionStrategy>();
        subBinSelection.Setup(x => x.Execute(It.IsAny<IEnumerable<BinType>>(), It.IsAny<List<Item>>()))
            .Returns<IEnumerable<BinType>, List<Item>>((bins, its) =>
            {
                var bin = bins.First();
                // اگر همه آیتم‌ها توی Bin جا میشن → Bin رو برگردون
                if (its.Any(i => i.Length <= bin.Length && i.Width <= bin.Width && i.Height <= bin.Height))
                    return bin;

                // در غیر این صورت → Bin انتخاب نشد
                return null;
            });


        var singleBinPacking = new Mock<ISingleBinPackingAlgorithm>();
        singleBinPacking.Setup(x => x.Execute(It.IsAny<List<Item>>(), binType))
            .Returns<List<Item>, BinType>((its, bin) =>
            {
                var packed = its.Where(i => i.Length <= bin.Length && i.Width <= bin.Width && i.Height <= bin.Height)
                                .Select(i => new PackedItemViewModel { ItemId = i.Id })
                                .ToList();

                var left = its.Where(i => i.Length > bin.Length || i.Width > bin.Width || i.Height > bin.Height)
                              .Select(i => new ItemViewModel { Id = i.Id })
                              .ToList();

                return new PackingResultViewModel
                {
                    PackedItems = packed,
                    LeftItems = left
                };
            });

        var algorithm = new PlacementAlgorithm(itemOrdering.Object, subBinSelection.Object, singleBinPacking.Object);

        // Act
        var result = algorithm.Execute(items, new List<BinType> { binType });

        // Assert
        Assert.Single(result.PackedItems);       // فقط آیتم کوچک جا شد
        Assert.Single(result.LeftItems);         // آیتم بزرگ باقی موند
        Assert.Single(result.UsedBinTypes);         // آیتم بزرگ باقی موند
        Assert.Equal(items[0].Id, result.PackedItems[0].ItemId);
        Assert.Equal(items[1].Id, result.LeftItems[0].Id);
    }



    [Fact]
    public void Execute_ShouldReturnEmptyResults_WhenNoBinAvailable()
    {
        // Arrange
        var items = new List<Item> { new Item(5, 5, 5) };

        var itemOrdering = new Mock<IItemOrderingStrategy>();
        itemOrdering.Setup(x => x.Apply(It.IsAny<IEnumerable<Item>>()))
            .Returns((IEnumerable<Item> it) => it);

        var subBinSelection = new Mock<ISubBinSelectionStrategy>();
        subBinSelection.Setup(x => x.Execute(It.IsAny<IEnumerable<BinType>>(), It.IsAny<List<Item>>()))
            .Returns((BinType?)null);

        var singleBinPacking = new Mock<ISingleBinPackingAlgorithm>();

        var algorithm = new PlacementAlgorithm(itemOrdering.Object, subBinSelection.Object, singleBinPacking.Object);

        // Act
        var result = algorithm.Execute(items, new List<BinType>());

        // Assert
        Assert.Empty(result.PackedItems);
        Assert.Empty(result.UsedBinTypes);
        Assert.Single(result.LeftItems);
    }
}
