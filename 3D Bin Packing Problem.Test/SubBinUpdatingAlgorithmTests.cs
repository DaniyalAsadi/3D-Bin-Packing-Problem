using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using FluentAssertions;
using System.Reflection;

namespace _3D_Bin_Packing_Problem.Test;
/// <summary>
/// Validates that the sub-bin updating algorithm correctly divides and merges sub-bin regions.
/// </summary>
public class SubBinUpdatingAlgorithmExtendedTests
{
    private readonly IPlacementFeasibilityChecker _checker = new PlacementFeasibilityChecker();
    private readonly ISubBinUpdatingAlgorithm _algorithm = new SubBinUpdatingAlgorithm();
    BinType binType = new BinType("Default", new Dimensions(1, 1, 1));

    // ---------------- تقسیم (Divide) ----------------

    [Fact]
    public void Execute_ShouldDivideInXRight_WhenItemStartsAtOriginAndCoversFullYAndZ()
    {
        var item = new Item(new Dimensions(2, 5, 5));
        var subBinList = new List<SubBin> { new(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0) };
        var placedBoxes = new List<PlacedBox>();
        var checkResult = _checker.Execute(binType, item, subBinList[0],placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        var result = _algorithm.Execute(subBinList, placementResult);
        result.Should().ContainSingle(sb =>
        sb.Position.X == 2 &&
        sb.Size.Length == 3);

    }

    [Fact]
    public void Execute_ShouldDivideInYFront_WhenItemCoversFullXAndZ()
    {
        var item = new Item(new Dimensions(5, 2, 5));
        var subBinList = new List<SubBin> { new(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0) };
        var placedBoxes = new List<PlacedBox>();
        var checkResult = _checker.Execute(binType, item, subBinList[0],placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        var result = _algorithm.Execute(subBinList, placementResult);
        result.Should().ContainSingle(sb =>
        sb.Position.Y == 2 &&
        sb.Size.Width == 3);

    }

    [Fact]
    public void Execute_ShouldDivideInZTop_WhenItemCoversFullXAndY()
    {
        var item = new Item(new Dimensions(5, 5, 2));
        var subBinList = new List<SubBin> { new(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0) };
        var placedBoxes = new List<PlacedBox>();
        var checkResult = _checker.Execute(binType, item, subBinList[0],placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        var result = _algorithm.Execute(subBinList, placementResult);
        result.Should().ContainSingle(sb =>
        sb.Position.Z == 2 &&
        sb.Size.Height == 3);

    }

    [Fact]
    public void Execute_ShouldLeaveNoSubBins_WhenItemFillsEntireSubBin()
    {
        var item = new Item(new Dimensions(5, 5, 5));
        var subBinList = new List<SubBin> { new(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0) };
        var placedBoxes = new List<PlacedBox>();
        var checkResult = _checker.Execute(binType, item, subBinList[0],placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        var result = _algorithm.Execute(subBinList, placementResult);
        result.Should().BeEmpty();

    }

    [Fact]
    public void Execute_ShouldDivideIntoThreeSubBins_WhenItemPlacedAtOrigin()
    {
        // Arrange
        var item = new Item(new Dimensions(2, 2, 2));
        var subBinList = new List<SubBin>
            {
                new(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0)
            };
        var placedBoxes = new List<PlacedBox>();


        // Act
        var checkResult = _checker.Execute(binType, item, subBinList[0],placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        var result = _algorithm.Execute(subBinList, placementResult);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);// Right, Front, Top

        // بررسی ابعاد هر SubBin
        var right = result.Should()
            .ContainSingle(sb => sb.Position.X == 2 && sb.Size.Length == 3)
            .Which;

        right.Size.Width.Should().Be(5);
        right.Size.Height.Should().Be(5);

        var front = result.Should()
            .ContainSingle(sb => sb.Position.Y == 2 && sb.Size.Width == 3)
            .Which;

        front.Size.Length.Should().Be(5);
        front.Size.Height.Should().Be(5);

        var top = result.Should()
            .ContainSingle(sb => sb.Position.Z == 2 && sb.Size.Height == 3)
            .Which;

        top.Size.Length.Should().Be(5);
        top.Size.Width.Should().Be(5);


    }

    [Fact]
    public void Execute_ShouldHandleTwoConsecutiveItems()
    {
        // Arrange
        var subBinList = new List<SubBin>
            {
                new(new Point3(0, 0, 0),new Dimensions( 5, 5, 5), 0, 0, 0, 0)
            };

        var item1 = new Item(new Dimensions(2, 2, 2));
        var item2 = new Item(new Dimensions(3, 3, 3));
        var placedBoxes = new List<PlacedBox>();

        // --- مرحله ۱ ---
        var checkResult = _checker.Execute(binType, item1, subBinList[0],placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        subBinList = _algorithm.Execute(subBinList, placementResult);


        // بررسی مرحله اول
        subBinList.Count.Should().Be(3);
        subBinList.Should().Contain(sb => sb.Position.X == 2 && sb.Size.Length == 3);
        subBinList.Should().Contain(sb => sb.Position.Y == 2 && sb.Size.Width == 3);
        subBinList.Should().Contain(sb => sb.Position.Z == 2 && sb.Size.Height == 3);



        // --- مرحله ۲ ---
        var checkResult2 = _checker.Execute(binType, item2, subBinList[0], [placementResult.PlacedBox], out var placementResult2);
        placementResult.Should().NotBeNull();
        var resultAfterSecond = _algorithm.Execute(subBinList, placementResult2);


        // ✅ بررسی منطقی خروجی مرحله دوم
        resultAfterSecond.Should().NotBeNull();

        resultAfterSecond.Should().AllSatisfy(sb =>
        {
            sb.Size.Length.Should().BeGreaterThan(0, "طول نباید صفر باشد");
            sb.Size.Width.Should().BeGreaterThan(0, "عرض نباید صفر باشد");
            sb.Size.Height.Should().BeGreaterThan(0, "ارتفاع نباید صفر باشد");
        });

        // ✅ هیچ SubBin اولیه‌ای نباید باقی مانده باشد
        resultAfterSecond.Should().NotContain(sb =>
            sb.Position.X == 0 &&
            sb.Position.Y == 0 &&
            sb.Position.Z == 0);

        // ✅ SubBinها باید در محدوده جدید باشند
        resultAfterSecond.Should().AllSatisfy(sb =>
        {
            sb.Position.X.Should().BeInRange(0, 5);
            sb.Position.Y.Should().BeInRange(0, 5);
            sb.Position.Z.Should().BeInRange(0, 5);
        });

    }


    // ---------------- همپوشانی (Overlap) ----------------


    [Fact]
    public void HasOverlap_ShouldReturnFalse_WhenNoOverlap()
    {
        var sb = new SubBin(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0);
        var placement = new PlacedBox(
            0,
            0,
            0,
            2,
            2,
            2
        );

        var result = InvokeHasOverlap(sb, placement);

        result.Should().BeTrue();
    }

    [Fact]
    public void HasOverlap_ShouldReturnTrue_WhenOverlapExists()
    {
        var sb = new SubBin(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0);
        var placement = new PlacedBox(
            0,
            0,
            0,
            2,
            2,
            2
        );
        var result = InvokeHasOverlap(sb, placement);

        result.Should().BeTrue();
    }

    // ---------------- دربرگیری (Containment) ----------------

    [Fact]
    public void IsContained_ShouldReturnTrue_WhenASubBinInsideB()
    {
        var a = new SubBin(new Point3(1, 1, 1), new Dimensions(2, 2, 2), 0, 0, 0, 0);
        var b = new SubBin(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0);

        var result = InvokeIsContained(a, b);

        result.Should().BeTrue();
    }

    [Fact]
    public void Execute_ShouldRemoveNewSubBin_WhenNewIsContainedInOld()
    {
        var item = new Item(new Dimensions(2, 2, 2));
        var subBinList = new List<SubBin>
        {
            new(new Point3(0, 0, 0), new Dimensions(10, 10, 10),0,0,0,0)
        };
        var placedBoxes = new List<PlacedBox>();

        var checkResult = _checker.Execute(binType, item, subBinList[0], placedBoxes, out var placementResult);
        placementResult.Should().NotBeNull();
        var result = _algorithm.Execute(subBinList, placementResult);

        foreach (var sb1 in result)
        {
            foreach (var sb2 in result)
            {
                if (sb1 != sb2)
                {
                    InvokeIsContained(sb1, sb2).Should().BeFalse();
                }
            }
        }
    }

    [Fact]
    public void Merge_ShouldRemoveOldSubBin_WhenOldInsideNew()
    {
        var oldSb = new SubBin(new Point3(2, 2, 2), new Dimensions(2, 2, 2), 0, 0, 0, 0);
        var newSb = new SubBin(new Point3(0, 0, 0), new Dimensions(10, 10, 10), 0, 0, 0, 0);

        InvokeIsContained(oldSb, newSb).Should().BeTrue();

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

        subBinList.Should().BeEmpty();
        newSubBinList.Should().ContainSingle();
    }

    [Fact]
    public void Merge_ShouldNotRemove_WhenNoContainment()
    {
        var sb1 = new SubBin(new Point3(0, 0, 0), new Dimensions(5, 5, 5), 0, 0, 0, 0);
        var sb2 = new SubBin(new Point3(6, 6, 6), new Dimensions(5, 5, 5), 0, 0, 0, 0);

        InvokeIsContained(sb1, sb2).Should().BeFalse();
        InvokeIsContained(sb2, sb1).Should().BeFalse();

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

        subBinList.Should().ContainSingle();
        newSubBinList.Should().ContainSingle();
    }

    // ---------------- سناریوی ترکیبی ----------------

    [Fact]
    public void Execute_ShouldHandleComplexSequentialPlacements()
    {
        var subBinList = new List<SubBin> { new(new Point3(0, 0, 0), new Dimensions(10, 10, 10), 0, 0, 0, 0) };

        var items = new[]
        {
            new Item(new Dimensions(4, 10, 10)),
            new Item(new Dimensions(6, 5, 5)),
            new Item(new Dimensions(2, 2, 2))
        };
        var placedBoxes = new List<PlacedBox>();

        foreach (var item in items)
        {

            var checkResult = _checker.Execute(binType, item, subBinList[0],placedBoxes, out var placementResult);
            placementResult.Should().NotBeNull();
            placedBoxes.Add(placementResult.PlacedBox);
            subBinList = _algorithm.Execute(subBinList, placementResult);

        }

        subBinList.Should().NotBeEmpty();
        (subBinList.Count > 1).Should().BeTrue();
    }

    // ---------------- Helpers ----------------

    private static bool InvokeHasOverlap(SubBin sb, PlacedBox box)
    {
        var method = typeof(SubBinUpdatingAlgorithm)
            .GetMethod("HasOverlap", BindingFlags.NonPublic | BindingFlags.Static)!;

        return (bool)method.Invoke(null, [sb, box])!;
    }

    private static bool InvokeIsContained(SubBin a, SubBin b)
    {
        var method = typeof(SubBinUpdatingAlgorithm)
            .GetMethod("IsContained", BindingFlags.NonPublic | BindingFlags.Static)!;

        return (bool)method.Invoke(null, [a, b])!;
    }
}