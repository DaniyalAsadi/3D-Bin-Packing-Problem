using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Services.InnerLayer.SUA;
using _3D_Bin_Packing_Problem.ViewModels;
using System.Numerics;
using System.Reflection;

namespace BinPacking.Tests;

public class SubBinUpdatingAlgorithmExtendedTests
{
    private readonly IPlacementFeasibilityChecker _checker = new PlacementFeasibilityChecker();
    private readonly ISubBinUpdatingAlgorithm _algorithm;

    public SubBinUpdatingAlgorithmExtendedTests()
    {
        _algorithm = new SubBinUpdatingAlgorithm(_checker);
    }

    // ---------------- تقسیم (Divide) ----------------

    [Fact]
    public void Execute_ShouldDivideInXRight_WhenItemStartsAtOriginAndCoversFullYAndZ()
    {
        var item = new Item(2, 5, 5);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var result = _algorithm.Execute(subBinList, item);

        Assert.Single(result);
        Assert.Contains(result, sb => sb.X == 2 && sb.Length == 3);
    }

    [Fact]
    public void Execute_ShouldDivideInYFront_WhenItemCoversFullXAndZ()
    {
        var item = new Item(5, 2, 5);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var result = _algorithm.Execute(subBinList, item);

        Assert.Single(result);
        Assert.Contains(result, sb => sb.Y == 2 && sb.Width == 3);
    }

    [Fact]
    public void Execute_ShouldDivideInZTop_WhenItemCoversFullXAndY()
    {
        var item = new Item(5, 5, 2);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var result = _algorithm.Execute(subBinList, item);

        Assert.Single(result);
        Assert.Contains(result, sb => sb.Z == 2 && sb.Height == 3);
    }

    [Fact]
    public void Execute_ShouldLeaveNoSubBins_WhenItemFillsEntireSubBin()
    {
        var item = new Item(5, 5, 5);
        var subBinList = new List<SubBin> { new(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0) };

        var result = _algorithm.Execute(subBinList, item);

        Assert.Empty(result);
    }

    // ---------------- همپوشانی (Overlap) ----------------

    [Fact]
    public void HasOverlap_ShouldReturnFalse_WhenNoOverlap()
    {
        var sb = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var placement = new PlacementResult(new Item(2, 2, 2), new Vector3(10, 10, 10), new Vector3(2, 2, 2), 1, 1);

        var result = InvokeHasOverlap(sb, placement);

        Assert.False(result);
    }

    [Fact]
    public void HasOverlap_ShouldReturnTrue_WhenOverlapExists()
    {
        var sb = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var placement = new PlacementResult(new Item(2, 2, 2), new Vector3(1, 1, 1), new Vector3(2, 2, 2), 1, 1);

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

        var result = _algorithm.Execute(subBinList, item);

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
            subBinList = _algorithm.Execute(subBinList, item);

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