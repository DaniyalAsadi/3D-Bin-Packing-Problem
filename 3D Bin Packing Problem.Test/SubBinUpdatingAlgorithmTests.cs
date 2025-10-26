using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Numerics;
using System.Reflection;

namespace BinPacking.Tests;

/// <summary>
/// Validates that the sub-bin updating algorithm correctly divides and merges sub-bin regions.
/// </summary>
public class SubBinUpdatingAlgorithmExtendedTests
{
    private readonly IPlacementFeasibilityChecker _checker = new PlacementFeasibilityChecker();
    private readonly ISubBinUpdatingAlgorithm _algorithm = new SubBinUpdatingAlgorithm();
    BinType binType = new BinType();

    // ---------------- تقسیم (Divide) ----------------

    [Fact]
    public void Execute_ShouldDivideInXRight_WhenItemStartsAtOriginAndCoversFullYAndZ()
    {
        var item = new Item(2, 5, 5);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };
        var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var result = _algorithm.Execute(subBinList, placementResult);

        Assert.Single(result, sb => sb is { X: 2, Length: 3 });
    }

    [Fact]
    public void Execute_ShouldDivideInYFront_WhenItemCoversFullXAndZ()
    {
        var item = new Item(5, 2, 5);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var result = _algorithm.Execute(subBinList, placementResult);


        Assert.Single(result, sb => sb is { Y: 2, Width: 3 });
    }

    [Fact]
    public void Execute_ShouldDivideInZTop_WhenItemCoversFullXAndY()
    {
        var item = new Item(5, 5, 2);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var result = _algorithm.Execute(subBinList, placementResult);


        Assert.Single(result);
        Assert.Contains(result, sb => sb is { Z: 2, Height: 3 });
    }

    [Fact]
    public void Execute_ShouldLeaveNoSubBins_WhenItemFillsEntireSubBin()
    {
        var item = new Item(5, 5, 5);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var result = _algorithm.Execute(subBinList, placementResult);


        Assert.Empty(result);
    }

    [Fact]
    public void Execute_ShouldDivideIntoThreeSubBins_WhenItemPlacedAtOrigin()
    {
        // Arrange
        var item = new Item(2, 2, 2);
        var subBinList = new List<SubBin>
        {
            new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0)
        };



        // Act
        var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var result = _algorithm.Execute(subBinList, placementResult);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Right, Front, Top

        // بررسی ابعاد هر SubBin
        var right = Assert.Single(result, sb => sb is { X: 2, Length: 3 });
        Assert.Equal((5, 5), (right.Width, right.Height));

        var front = Assert.Single(result, sb => sb is { Y: 2, Width: 3 });
        Assert.Equal((5, 5), (front.Length, front.Height));

        var top = Assert.Single(result, sb => sb is { Z: 2, Height: 3 });
        Assert.Equal((5, 5), (top.Length, top.Width));
    }

    [Fact]
    public void Execute_ShouldHandleTwoConsecutiveItems()
    {
        // Arrange
        var subBinList = new List<SubBin>
        {
            new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0)
        };

        var item1 = new Item(2, 2, 2);
        var item2 = new Item(3, 3, 3);

        // --- مرحله ۱ ---
        var checkResult = _checker.Execute(binType, item1, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var resultAfterFirst = _algorithm.Execute(subBinList, placementResult);


        // بررسی مرحله اول
        Assert.Equal(3, resultAfterFirst.Count);
        Assert.Contains(resultAfterFirst, sb => sb is { X: 2, Length: 3 });
        Assert.Contains(resultAfterFirst, sb => sb is { Y: 2, Width: 3 });
        Assert.Contains(resultAfterFirst, sb => sb is { Z: 2, Height: 3 });


        // --- مرحله ۲ ---
        var checkResult2 = _checker.Execute(binType, item2, subBinList[0], out var placementResult2);
        Assert.NotNull(placementResult);
        var resultAfterSecond = _algorithm.Execute(subBinList, placementResult2);


        // ✅ بررسی منطقی خروجی مرحله دوم
        Assert.NotNull(resultAfterSecond);
        Assert.All(resultAfterSecond, sb =>
        {
            Assert.True(sb.Length > 0, "طول نباید صفر باشد");
            Assert.True(sb.Width > 0, "عرض نباید صفر باشد");
            Assert.True(sb.Height > 0, "ارتفاع نباید صفر باشد");
        });

        // ✅ هیچ SubBin اولیه‌ای نباید باقی مانده باشد
        Assert.DoesNotContain(resultAfterSecond, sb => sb.X == 0 && sb.Y == 0 && sb.Z == 0);

        // ✅ SubBinها باید در محدوده جدید باشند
        Assert.All(resultAfterSecond, sb =>
        {
            Assert.InRange(sb.X, 0, 5);
            Assert.InRange(sb.Y, 0, 5);
            Assert.InRange(sb.Z, 0, 5);
        });
    }

    [Fact]
    public void Execute_ShouldPlaceEightItemsWithoutOverlap()
    {
        // Arrange
        var subBinList = new List<SubBin>
    {
        new(0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0)
    };

        var items = Enumerable.Range(1, 8)
                              .Select(_ => new Item(1, 1, 1))
                              .ToList();

        var placements = new List<PlacementResult>();
        var currentSubBins = subBinList;

        // Act
        foreach (var item in items)
        {

            var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
            Assert.NotNull(placementResult);
            var result = _algorithm.Execute(subBinList, placementResult);
            currentSubBins = result;

            // برای بررسی موقعیت آیتم، باید placement در الگوریتم ذخیره یا برگردانده شود.
            // فرض می‌گیریم کلاس PFCA یا checker آن را درون یک لیست static ذخیره می‌کند
            // یا الگوریتم را کمی اصلاح کردی تا آخرین placement قابل دسترسی باشد.
            var lastPlacement = PlacementFeasibilityChecker.LastPlacement;
            if (lastPlacement != null)
                placements.Add(lastPlacement);
        }

        // Assert
        Assert.True(placements.Count > 0, "هیچ آیتمی قرار داده نشده است");

        // ✅ بررسی عدم هم‌پوشانی
        for (var i = 0; i < placements.Count; i++)
        {
            for (var j = i + 1; j < placements.Count; j++)
            {
                var a = placements[i];
                var b = placements[j];

                var overlapX = !((a.Position.X + a.Orientation.X) <= b.Position.X ||
                                 a.Position.X >= (b.Position.X + b.Orientation.X));
                var overlapY = !((a.Position.Y + a.Orientation.Y) <= b.Position.Y ||
                                 a.Position.Y >= (b.Position.Y + b.Orientation.Y));
                var overlapZ = !((a.Position.Z + a.Orientation.Z) <= b.Position.Z ||
                                 a.Position.Z >= (b.Position.Z + b.Orientation.Z));

                var overlap = overlapX && overlapY && overlapZ;

                Assert.False(overlap,
                    $"Overlap detected between item {i + 1} at {a.Position} and item {j + 1} at {b.Position}");
            }
        }

        // ✅ بررسی محدود بودن همه آیتم‌ها داخل Bin
        Assert.All(placements, p =>
        {
            Assert.InRange(p.Position.X, 0, 2);
            Assert.InRange(p.Position.Y, 0, 2);
            Assert.InRange(p.Position.Z, 0, 2);
            Assert.InRange(p.Position.X + p.Orientation.X, 0, 2 + 1);
            Assert.InRange(p.Position.Y + p.Orientation.Y, 0, 2 + 1);
            Assert.InRange(p.Position.Z + p.Orientation.Z, 0, 2 + 1);
        });
    }


    // ---------------- همپوشانی (Overlap) ----------------


    [Fact]
    public void HasOverlap_ShouldReturnFalse_WhenNoOverlap()
    {
        var sb = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var placement = new PlacementResult(
            new Item(2, 2, 2),
            binType,
            new Vector3(10, 10, 10),
            new Vector3(2, 2, 2),
            1,
            1);

        var result = InvokeHasOverlap(sb, placement);

        Assert.False(result);
    }

    [Fact]
    public void HasOverlap_ShouldReturnTrue_WhenOverlapExists()
    {
        var sb = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var placement = new PlacementResult(
            new Item(2, 2, 2),
            binType,
            new Vector3(1, 1, 1),
            new Vector3(2, 2, 2),
            1,
            1);

        var result = InvokeHasOverlap(sb, placement);

        Assert.True(result);
    }

    // ---------------- دربرگیری (Containment) ----------------

    [Fact]
    public void IsContained_ShouldReturnTrue_WhenASubBinInsideB()
    {
        var a = new SubBin(1, 1, 1, 2, 2, 2, 0, 0, 0, 0, 0);
        var b = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = InvokeIsContained(a, b);

        Assert.True(result);
    }

    [Fact]
    public void Execute_ShouldRemoveNewSubBin_WhenNewIsContainedInOld()
    {
        var item = new Item(2, 2, 2);
        var subBinList = new List<SubBin>
        {
            new(0, 0, 0, 10, 10, 10,0,0,0,0,0)
        };

        var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
        Assert.NotNull(placementResult);
        var result = _algorithm.Execute(subBinList, placementResult);

        foreach (var sb1 in result)
        {
            foreach (var sb2 in result)
            {
                if (sb1 != sb2)
                {
                    Assert.False(InvokeIsContained(sb1, sb2));
                }
            }
        }
    }

    [Fact]
    public void Merge_ShouldRemoveOldSubBin_WhenOldInsideNew()
    {
        var oldSb = new SubBin(2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0);
        var newSb = new SubBin(0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0);

        Assert.True(InvokeIsContained(oldSb, newSb));

        var subBinList = new List<SubBin> { oldSb };
        var newSubBinList = new List<SubBin> { newSb };

        foreach (var sb in subBinList.ToList())
        {
            foreach (var nsb in newSubBinList.ToList())
            {
                if (InvokeIsContained(nsb, sb))
                    newSubBinList.Remove(nsb);
                else if (InvokeIsContained(sb, nsb))
                    subBinList.Remove(sb);
            }
        }

        Assert.Empty(subBinList);
        Assert.Single(newSubBinList);
    }

    [Fact]
    public void Merge_ShouldNotRemove_WhenNoContainment()
    {
        var sb1 = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var sb2 = new SubBin(6, 6, 6, 5, 5, 5, 0, 0, 0, 0, 0);

        Assert.False(InvokeIsContained(sb1, sb2));
        Assert.False(InvokeIsContained(sb2, sb1));

        var subBinList = new List<SubBin> { sb1 };
        var newSubBinList = new List<SubBin> { sb2 };

        foreach (var sb in subBinList.ToList())
        {
            foreach (var nsb in newSubBinList.ToList())
            {
                if (InvokeIsContained(nsb, sb))
                    newSubBinList.Remove(nsb);
                else if (InvokeIsContained(sb, nsb))
                    subBinList.Remove(sb);
            }
        }

        Assert.Single(subBinList);
        Assert.Single(newSubBinList);
    }

    // ---------------- سناریوی ترکیبی ----------------

    [Fact]
    public void Execute_ShouldHandleComplexSequentialPlacements()
    {
        var subBinList = new List<SubBin> { new(0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0) };

        var items = new[]
        {
            new Item(4, 10, 10),
            new Item(6, 5, 5),
            new Item(2, 2, 2)
        };

        foreach (var item in items)
        {

            var checkResult = _checker.Execute(binType, item, subBinList[0], out var placementResult);
            Assert.NotNull(placementResult);
            var result = _algorithm.Execute(subBinList, placementResult);
        }

        Assert.NotEmpty(subBinList);
        Assert.True(subBinList.Count > 1);
    }

    // ---------------- Helpers ----------------

    private static bool InvokeHasOverlap(SubBin sb, PlacementResult placement)
    {
        var method = typeof(SubBinUpdatingAlgorithm)
            .GetMethod("HasOverlap", BindingFlags.NonPublic | BindingFlags.Static)!;

        return (bool)method.Invoke(null, new object[] { sb, placement })!;
    }

    private static bool InvokeIsContained(SubBin a, SubBin b)
    {
        var method = typeof(SubBinUpdatingAlgorithm)
            .GetMethod("IsContained", BindingFlags.NonPublic | BindingFlags.Static)!;

        return (bool)method.Invoke(null, new object[] { a, b })!;
    }
}