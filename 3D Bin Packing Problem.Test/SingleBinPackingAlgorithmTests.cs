using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SPA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderingStrategy;
using _3D_Bin_Packing_Problem.ViewModels;
using Moq;
using System.Numerics;

namespace BinPacking.Tests;

public class SingleBinPackingAlgorithmTests
{
    [Fact]
    public void Execute_ShouldPackItem_WhenFeasible()
    {
        // Arrange
        var item = new Item(2, 2, 2);
        var binType = new BinType { Length = 10, Width = 10, Height = 10 };

        var expectedPlacement = new PlacementResult(
            item,
            new Vector3(0, 0, 0),
            new Vector3(2, 2, 2),
            1.0,
            0.75
        );

        var feasibilityChecker = new Mock<IPlacementFeasibilityChecker>();
        feasibilityChecker
            .Setup(fc => fc.Execute(item, It.IsAny<SubBin>(), out It.Ref<PlacementResult>.IsAny!))
            .Returns((Item i, SubBin sb, out PlacementResult result) =>
            {
                result = expectedPlacement; // مقداردهی به out
                return true; // آیتم قابل جاگذاری است
            });

        var subBinUpdatingAlgorithm = new Mock<ISubBinUpdatingAlgorithm>();
        var subBinOrderStrategy = new Mock<ISubBinOrderingStrategy>();

        // اینجا مثلا بدون تغییر SubBin لیست را برمی‌گردونیم
        subBinOrderStrategy
            .Setup(s => s.Apply(It.IsAny<IEnumerable<SubBin>>(), It.IsAny<Item>()))
            .Returns<IEnumerable<SubBin>, Item>((bins, it) => bins);

        var algorithm = new SingleBinPackingAlgorithm(
            feasibilityChecker.Object,
            subBinUpdatingAlgorithm.Object,
            subBinOrderStrategy.Object
        );

        // Act
        var result = algorithm.Execute(new List<Item> { item }, binType);

        // Assert
        Assert.Single(result.PackedItems); // فقط یک آیتم باید بسته‌بندی شود
        Assert.Empty(result.LeftItems);    // نباید آیتمی باقی بماند

        var packed = result.PackedItems.First();
        Assert.Equal(item.Id, packed.ItemId);
        Assert.Equal(0, packed.X);
        Assert.Equal(0, packed.Y);
        Assert.Equal(0, packed.Z);
        Assert.Equal(2, packed.Length);
        Assert.Equal(2, packed.Width);
        Assert.Equal(2, packed.Height);
        Assert.Equal(0.75, packed.SupportRatio);
    }

    // تست: آیتم بزرگ‌تر از Bin است → باید در LeftItems قرار گیرد
    [Fact]
    public void Execute_ShouldLeaveItem_WhenTooLarge()
    {
        // Arrange
        var item = new Item(20, 20, 20); // بزرگ‌تر از Bin
        var binType = new BinType { Length = 10, Width = 10, Height = 10 };

        var feasibilityChecker = new Mock<IPlacementFeasibilityChecker>();
        PlacementResult? dummy = null;
        feasibilityChecker
            .Setup(fc => fc.Execute(item, It.IsAny<SubBin>(), out dummy))
            .Returns(false);

        var subBinUpdatingAlgorithm = new Mock<ISubBinUpdatingAlgorithm>();
        var subBinOrderStrategy = new Mock<ISubBinOrderingStrategy>();

        var algorithm = new SingleBinPackingAlgorithm(feasibilityChecker.Object, subBinUpdatingAlgorithm.Object, subBinOrderStrategy.Object);

        // Act
        var result = algorithm.Execute(new List<Item> { item }, binType);

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

        var binType = new BinType { Length = 10, Width = 10, Height = 10 };

        var placementResult = new PlacementResult(
            items[0],
            new Vector3(0, 0, 0),
            new Vector3(5, 5, 5),
            1.0,
            0.75
        );

        var feasibilityChecker = new Mock<IPlacementFeasibilityChecker>();

        // آیتم اول جا میشه
        feasibilityChecker
            .Setup(fc => fc.Execute(items[0], It.IsAny<SubBin>(), out It.Ref<PlacementResult>.IsAny!))
            .Returns((Item i, SubBin sb, out PlacementResult result) =>
            {
                result = placementResult;
                return true;
            });

        // آیتم دوم جا نمیشه
        feasibilityChecker
            .Setup(fc => fc.Execute(items[1], It.IsAny<SubBin>(), out It.Ref<PlacementResult>.IsAny!))
            .Returns((Item i, SubBin sb, out PlacementResult result) =>
            {
                result = null!;
                return false;
            });

        var subBinUpdatingAlgorithm = new Mock<ISubBinUpdatingAlgorithm>();
        var subBinOrderStrategy = new Mock<ISubBinOrderingStrategy>();

        // پاس‌ترو برای استراتژی مرتب‌سازی
        subBinOrderStrategy
            .Setup(s => s.Apply(It.IsAny<IEnumerable<SubBin>>(), It.IsAny<Item>()))
            .Returns<IEnumerable<SubBin>, Item>((bins, it) => bins);

        var algorithm = new SingleBinPackingAlgorithm(
            feasibilityChecker.Object,
            subBinUpdatingAlgorithm.Object,
            subBinOrderStrategy.Object
        );

        // Act
        var result = algorithm.Execute(items, binType);

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

        var validSubBin = new SubBin(0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0);
        var tooSmallSubBin = new SubBin(0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0); // نباید انتخاب بشه

        var subBins = new List<SubBin> { validSubBin, tooSmallSubBin };

        var feasibilityChecker = new Mock<IPlacementFeasibilityChecker>();
        var subBinUpdatingAlgorithm = new Mock<ISubBinUpdatingAlgorithm>();
        var subBinOrderStrategy = new Mock<ISubBinOrderingStrategy>();

        var algorithm = new SingleBinPackingAlgorithm(feasibilityChecker.Object, subBinUpdatingAlgorithm.Object, subBinOrderStrategy.Object);

        // Act (استفاده از متد Private با Reflection یا تغییر دسترسی)
        var method = typeof(SingleBinPackingAlgorithm)
            .GetMethod("ApplySpeedUpStrategy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = method?.Invoke(algorithm, new object[] { subBins, items }) as List<SubBin>;

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); // فقط یک sub-bin معتبر باید باقی بماند
        Assert.Equal(validSubBin, result![0]);
    }
    [Fact]
    public void Execute_ShouldPackEightSmallItemsIntoBin_WhenFeasible()
    {
        // Arrange
        var items = Enumerable.Range(0, 8)
            .Select(_ => new Item(1, 1, 1))
            .ToList();

        var binType = new BinType { Length = 2, Width = 2, Height = 2 };

        var placements = new Queue<PlacementResult>();

        // Generate expected placements (2×2×2 bin has 8 unit slots)
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {
                    placements.Enqueue(new PlacementResult(
                        new Item(1, 1, 1),
                        new Vector3(x, y, z),   // placement corner
                        new Vector3(1, 1, 1),   // dimensions
                        1.0,
                        0.75
                    ));
                }
            }
        }

        var feasibilityChecker = new Mock<IPlacementFeasibilityChecker>();
        feasibilityChecker
            .Setup(fc => fc.Execute(It.IsAny<Item>(), It.IsAny<SubBin>(), out It.Ref<PlacementResult>.IsAny!))
            .Returns((Item i, SubBin sb, out PlacementResult result) =>
            {
                if (placements.Count > 0)
                {
                    result = placements.Dequeue();
                    return true;
                }

                result = null!;
                return false;
            });

        var subBinUpdatingAlgorithm = new Mock<ISubBinUpdatingAlgorithm>();
        var subBinOrderStrategy = new Mock<ISubBinOrderingStrategy>();
        subBinOrderStrategy
            .Setup(s => s.Apply(It.IsAny<IEnumerable<SubBin>>(), It.IsAny<Item>()))
            .Returns<IEnumerable<SubBin>, Item>((bins, it) => bins);

        var algorithm = new SingleBinPackingAlgorithm(
            feasibilityChecker.Object,
            subBinUpdatingAlgorithm.Object,
            subBinOrderStrategy.Object
        );

        // Act
        var result = algorithm.Execute(items, binType);

        // Assert
        Assert.Equal(8, result.PackedItems.Count); // همه ۸ آیتم باید بسته شوند
        Assert.Empty(result.LeftItems);            // آیتمی نباید باقی بماند

        // بررسی اینکه همه نقاط پوشش داده شدند
        var coords = result.PackedItems.Select(p => (p.X, p.Y, p.Z)).ToHashSet();
        Assert.Equal(8, coords.Count);
        Assert.Contains((0, 0, 0), coords);
        Assert.Contains((1, 0, 0), coords);
        Assert.Contains((0, 1, 0), coords);
        Assert.Contains((1, 1, 0), coords);
        Assert.Contains((0, 0, 1), coords);
        Assert.Contains((1, 0, 1), coords);
        Assert.Contains((0, 1, 1), coords);
        Assert.Contains((1, 1, 1), coords);
    }

}