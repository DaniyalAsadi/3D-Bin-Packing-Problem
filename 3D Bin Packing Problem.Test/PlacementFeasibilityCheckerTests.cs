using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Extensions;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using FluentAssertions;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Test;

/// <summary>
/// Provides unit tests for validating the placement feasibility checker logic.
/// </summary>
public class PlacementFeasibilityCheckerTests
{
    private readonly PlacementFeasibilityChecker _checker = new();
    private const float Eps = AppConstants.Tolerance;

    /// <summary>
    /// اگر حجم آیتم از حجم SubBin بیشتر باشد باید False برگرداند
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemVolumeGreaterThanSubBin()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(10, 10, 10)); // حجم = 1000
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5)); // حجم = 125

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }





    /// <summary>
    /// اگر آیتم دقیقاً داخل SubBin جا شود باید True برگرداند
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnTrue_WhenItemFitsExactly()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(2, 2, 2));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeTrue();
        placement.Should().NotBeNull();
        placement.Item.Should().Be(item);
    }

    /// <summary>
    /// اگر نسبت پشتیبانی (SupportRatio) کمتر از آستانه λ باشد آیتم قرار نمی‌گیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenSupportRatioTooLow()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(5, 5, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(6, 2, 1));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }

    /// <summary>
    /// متد GetPoints باید حداکثر ۵ نقطه‌ی کلیدی روی کف برگرداند
    /// </summary>
    [Fact]
    public void GetPoints_ShouldReturnFiveUniquePoints()
    {
        var item = new Item(new Dimensions(2, 2, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var points = InvokeGetPoints(subBin, 2, 2, 0.75);

        points.Should().NotBeEmpty();
        (points.Count <= 5).Should().BeTrue();
        points.Should().AllSatisfy(p =>
        {
            p.Z.Should().Be(0);
        });
    }

    /// <summary>
    /// بین چند حالت ممکن، بهترین Placement با کمترین Margin انتخاب می‌شود
    /// </summary>
    [Fact]
    public void Execute_ShouldChooseBestMarginPlacement()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(2, 2, 2));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(6, 6, 6));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeTrue();
        placement.Should().NotBeNull();
        placement.SmallestMargin.Should().BeGreaterThanOrEqualTo(0);
        placement.SupportRatio.Should().BeGreaterThanOrEqualTo(0.75);
    }

    /// <summary>
    /// اگر آیتم از مرز SubBin بیرون بزند باید False برگردد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemCollidesWithBoundary()
    {
        var binType = new BinType("Default", 1, 1, 1);

        var item = new Item(new Dimensions(10, 1, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }

    /// <summary>
    /// اگر آیتم در هیچ حالتی جا نشود باید لیست نقاط خالی برگردد
    /// </summary>
    [Fact]
    public void GetPoints_ShouldReturnEmpty_WhenItemDoesNotFit()
    {
        var item = new Item(new Dimensions(10, 10, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var points = InvokeGetPoints(subBin, 10, 10, 0.75);

        points.Should().BeEmpty();
    }

    /// <summary>
    /// اگر سطح پشتیبانی کافی نباشد، GetPoints باید نقطه‌ی جایگزین بازگرداند
    /// </summary>
    [Fact]
    public void GetPoints_ShouldFallback_WhenSupportAreaNotEnough()
    {
        var item = new Item(new Dimensions(5, 5, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var points = InvokeGetPoints(subBin, 5, 5, 0.9);

        points.Should().ContainSingle();
        points.Should().HaveElementAt(0, new Vector3(0, 0, 0));
    }

    /// <summary>
    /// تست محاسبه‌ی سطح پشتیبانی در حالتی که آیتم بخشی روی کف قرار دارد
    /// </summary>
    [Fact]
    public void ComputeSupportArea_ShouldBePartial_WhenItemOverhangs()
    {
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));
        var placedBox = new PlacedBox(x: 3, y: 0, z: 0, l: 5, w: 2, h: 1);

        var result = InvokeComputeSupportArea(subBin, placedBox);

        result.Should().Be(4);
    }

    /// <summary>
    /// اگر آیتم چندین اورینتیشن معتبر داشته باشد باید یکی از آن‌ها انتخاب شود
    /// </summary>
    [Fact]
    public void Execute_ShouldSelectCorrectOrientation_WhenMultipleOrientationsPossible()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(2, 3, 4));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeTrue();
        placement.Should().NotBeNull();
        item.GetOrientations().Should().Contain(placement!.Orientation);
    }

    /// <summary>
    /// اگر کوچکترین Margin برابر صفر باشد هم باید Placement معتبر باشد
    /// </summary>
    [Fact]
    public void Execute_ShouldAllowPlacement_WhenMarginEqualsZero()
    {
        var binType = new BinType("Default", 1, 1, 1);

        var item = new Item(new Dimensions(5, 5, 5));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeTrue();
        placement.Should().NotBeNull();
        Math.Abs(placement!.SmallestMargin).Should().BeLessThan(1e-5);
    }

    /// <summary>
    /// باید مقادیر Custom Margin در SubBin رعایت شوند
    /// </summary>
    [Fact]
    public void Execute_ShouldRespectCustomMargins()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(2, 2, 2));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5), 2, 2, 1, 1);

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeTrue();
        placement.Should().NotBeNull();
        placement!.SmallestMargin.Should().BeGreaterThanOrEqualTo(0);
    }

    /// <summary>
    /// اگر آیتم کاملاً داخل SubBin قرار گیرد سطح پشتیبانی باید کامل باشد
    /// </summary>
    [Fact]
    public void ComputeSupportArea_ShouldBeFull_WhenItemCompletelyInside()
    {
        var sb = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));
        var pb = new PlacedBox(x: 1, y: 1, z: 0, l: 3, w: 3, h: 1);

        var result = InvokeComputeSupportArea(sb, pb);
        result.Should().Be(9);
    }

    /// <summary>
    /// اگر هیچ همپوشانی وجود نداشته باشد سطح پشتیبانی صفر است
    /// </summary>
    [Fact]
    public void ComputeSupportArea_ShouldBeZero_WhenNoOverlap()
    {
        var sb = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));
        var pb = new PlacedBox(x: 10, y: 10, z: 0, l: 2, w: 2, h: 1);

        var result = InvokeComputeSupportArea(sb, pb);
        result.Should().Be(0);

    }


    /// <summary>
    /// اگر طول آیتم از طول SubBin بیشتر باشد باید قرار نگیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemExceedsXBoundary()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(6, 1, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }

    /// <summary>
    /// اگر عرض آیتم از عرض SubBin بیشتر باشد باید قرار نگیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemExceedsYBoundary()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(1, 6, 1));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }

    /// <summary>
    /// اگر ارتفاع آیتم از ارتفاع SubBin بیشتر باشد باید قرار نگیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemExceedsZBoundary()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(1, 1, 6));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(5, 5, 5));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }

    /// <summary>
    /// اگر SmallestMargin منفی شود یعنی آیتم بیرون زده و نباید قرار گیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldContinue_WhenSmallestMarginNegative()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var item = new Item(new Dimensions(3, 3, 3));
        var subBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(3, 2, 3));

        var result = _checker.Execute(binType, item, subBin, out var placement);

        result.Should().BeFalse();
        placement.Should().BeNull();
    }



    /// <summary>
    /// تست قرارگیری آیتم روی آیتم دیگر (Stacking) با رعایت SupportRatio ≥ λ
    /// </summary>
    [Fact]
    public void Execute_ShouldAllowStacking_WhenSupportRatioIsSatisfied()
    {
        var binType = new BinType("Default", 1, 1, 1);
        var baseSubBin = new SubBin(new Vector3(0, 0, 0), new Dimensions(10, 10, 10));

        var bottomItem = new Item(new Dimensions(10, 10, 2));
        var result1 = _checker.Execute(binType, bottomItem, baseSubBin, out var placement1);

        result1.Should().BeTrue();
        placement1.Should().NotBeNull();

        var binType2 = new BinType("Default", 1, 1, 1);
        var topSubBin = new SubBin(new Vector3(0, 0, 2), new Dimensions(10, 10, 8));

        var topItem = new Item(new Dimensions(5, 5, 2));
        var result2 = _checker.Execute(binType2, topItem, topSubBin, out var placement2);

        result2.Should().BeTrue();
        placement2.Should().NotBeNull();
        placement2.SupportRatio.Should().BeGreaterThanOrEqualTo(0.75);

    }

    private static float InvokeComputeSupportArea(SubBin sb, PlacedBox pb)
    {
        var method = typeof(PlacementFeasibilityChecker)
            .GetMethod("ComputeSupportArea", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;

        return (float)method.Invoke(null, new object[] { sb, pb })!;
    }
    private static IReadOnlyList<Vector3> InvokeGetPoints(SubBin sb, int L, int W, double lambda)
    {
        var method = typeof(PlacementFeasibilityChecker)
            .GetMethod("GetKeyPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;

        return (IReadOnlyList<Vector3>)method.Invoke(null, new object[] { sb, L, W, lambda })!;
    }
}