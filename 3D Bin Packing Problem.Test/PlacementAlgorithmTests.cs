// Pseudocode / Plan (detailed):
// 1. Test: When SubBinSelectionStrategy returns null immediately:
//    - Arrange: two items, mock item ordering returns same items,
//      mock sub-bin selection strategy returns null,
//      single-bin packing algorithm should not be invoked (or can return default).
//    - Act: call PlacementAlgorithm.Execute(items, bins).
//    - Assert: result.LeftItems contains both items (by Id),
//      result.PackedItems is empty, result.UsedBinTypes is empty.
//
// 2. Test: When SubBinSelectionStrategy returns a bin first and then null:
//    - Arrange: two items (A and B). Mock ordering returns same order.
//      Setup sub-bin selection strategy to return a BinType on first call and null on second call.
//      Mock single-bin packing algorithm to return a PackingResultViewModel that leaves B unpacked.
//    - Act: call Execute.
//    - Assert: result.UsedBinTypes contains the returned BinType,
//      result.LeftItems contains only B (by Id),
//      result.PackedItems contains whatever was returned by single-bin packing (empty or as arranged).
//
// Implementation notes:
// - Use Moq to stub factories/strategies and the single-bin packing algorithm.
// - Use SetupSequence for sub-bin selection to simulate multiple iterations.
// - Construct Item and ItemViewModel objects with stable Guid Ids to assert identity.
using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using FluentAssertions;
using Moq;

namespace _3D_Bin_Packing_Problem.Test;
public class PlacementAlgorithmBehaviorTests
{
    [Fact]
    public void Execute_WhenNoBinAvailable_AllItemsMarkedLeft()
    {
        // Arrange
        var itemA = new Item(new Dimensions(1, 1, 1));
        var itemB = new Item(new Dimensions(2, 2, 2));
        var items = new List<Item> { itemA, itemB };
        var bins = new List<BinType>(); // empty or irrelevant for this test

        // Mock ordering strategy
        var mockOrderingStrategy = new Mock<IItemOrderingStrategy>();
        mockOrderingStrategy
            .Setup(s => s.Apply(It.IsAny<IEnumerable<Item>>()))
            .Returns((IEnumerable<Item> src) => src);

        var mockOrderingFactory = new ItemOrderingStrategyFactory();


        // Mock sub-bin selection strategy to return null (no bin available)
        var mockSubStrategy = new Mock<ISubBinSelectionStrategy>();
        mockSubStrategy
            .Setup(s => s.Execute(It.IsAny<IEnumerable<BinType>>(), It.IsAny<List<Item>>()))
            .Returns((BinType?)null);

        var mockSubFactory = new SubBinSelectionStrategyFactory();


        // Mock single-bin packing algorithm (should not be invoked, but provide a stub)
        var mockSingleBin = new Mock<ISingleBinPackingAlgorithm>();

        var alg = new PlacementAlgorithm(
            mockOrderingFactory,
            mockSubFactory,
            mockSingleBin.Object
        );

        // Act
        var result = alg.Execute(items, bins);

        // Assert
        result.LeftItems.Count.Should().Be(2);
        var leftIds = result.LeftItems.Select(l => l.Id).ToHashSet();
        leftIds.Should().Contain(itemA.Id);
        leftIds.Should().Contain(itemB.Id);
        result.PackedItems.Should().BeEmpty();
        result.UsedBinTypes.Should().BeEmpty();
    }

    [Fact]
    public void Execute_WhenOneBinUsed_ShouldReportPackedAndLeftItems_Correctly()
    {
        // Arrange
        var itemA = new Item(new Dimensions(1, 1, 1));
        var itemB = new Item(new Dimensions(2, 2, 2));
        var items = new List<Item> { itemA, itemB };

        // Bin can fit only itemA
        var bin = new BinType("Default", new Dimensions(1, 1, 1));
        var bins = new List<BinType> { bin };

        // Mock ordering strategy: returns items as-is
        var mockOrderingStrategy = new Mock<IItemOrderingStrategy>();
        mockOrderingStrategy
            .Setup(s => s.Apply(It.IsAny<IEnumerable<Item>>()))
            .Returns((IEnumerable<Item> src) => src);

        var orderingFactory = new ItemOrderingStrategyFactory();

        // Mock sub-bin selection: always returns the bin
        var mockSubStrategy = new Mock<ISubBinSelectionStrategy>();
        mockSubStrategy
            .Setup(s => s.Execute(It.IsAny<IEnumerable<BinType>>(), It.IsAny<List<Item>>()))
            .Returns(bin);

        var subFactory = new SubBinSelectionStrategyFactory();

        // Mock single-bin packing: only packs itemA, leaves itemB
        var packingResult = new PackingResultViewModel
        {
            PackedItems = [new PackedItemViewModel { ItemId = itemA.Id }],
            LeftItems = [new ItemViewModel { Id = itemB.Id }],
            RemainingSubBins = []
        };

        var mockSingleBin = new Mock<ISingleBinPackingAlgorithm>();
        mockSingleBin
            .Setup(s => s.Execute(It.IsAny<List<Item>>(), It.IsAny<BinInstance>()))
            .Returns(packingResult);

        var algorithm = new PlacementAlgorithm(orderingFactory, subFactory, mockSingleBin.Object);

        // Act
        var result = algorithm.Execute(items, bins);

        // Assert
        // Bin was used
        result.UsedBinTypes.Should().ContainSingle();
        bin.Should().BeSameAs(result.UsedBinTypes[0].BinType);

        // Packed items: only itemA
        result.PackedItems.Should().ContainSingle();
        result.PackedItems[0].ItemId.Should().Be(itemA.Id);

        // Left items: only itemB
        result.LeftItems.Should().ContainSingle();
        result.LeftItems[0].Id.Should().Be(itemB.Id);
    }

}
