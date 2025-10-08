using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;
using System.Numerics;

namespace BinPacking.Tests;

/// <summary>
/// Provides unit tests for validating the placement feasibility checker logic.
/// </summary>
public class PlacementFeasibilityCheckerTests
{
    private readonly PlacementFeasibilityChecker _checker = new();

    /// <summary>
    /// اگر حجم آیتم از حجم SubBin بیشتر باشد باید False برگرداند
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemVolumeGreaterThanSubBin()
    {
        var item = new Item(10, 10, 10); // حجم = 1000
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0); // حجم = 125

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }

    /// <summary>
    /// اگر آیتم دقیقاً داخل SubBin جا شود باید True برگرداند
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnTrue_WhenItemFitsExactly()
    {
        var item = new Item(2, 2, 2);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.True(result);
        Assert.NotNull(placement);
        Assert.Equal(item, placement!.Item);
    }

    /// <summary>
    /// اگر نسبت پشتیبانی (SupportRatio) کمتر از آستانه λ باشد آیتم قرار نمی‌گیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenSupportRatioTooLow()
    {
        var item = new Item(5, 5, 1);
        var subBin = new SubBin(0, 0, 0, 6, 2, 1, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }

    /// <summary>
    /// متد GetPoints باید حداکثر ۵ نقطه‌ی کلیدی روی کف برگرداند
    /// </summary>
    [Fact]
    public void GetPoints_ShouldReturnFiveUniquePoints()
    {
        var item = new Item(2, 2, 1);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var points = PlacementFeasibilityChecker.GetPoints(subBin, item, 0.75);

        Assert.NotEmpty(points);
        Assert.True(points.Count <= 5);
        Assert.All(points, p => Assert.True(p.Z == 0));
    }

    /// <summary>
    /// بین چند حالت ممکن، بهترین Placement با کمترین Margin انتخاب می‌شود
    /// </summary>
    [Fact]
    public void Execute_ShouldChooseBestMarginPlacement()
    {
        var item = new Item(2, 2, 2);
        var subBin = new SubBin(0, 0, 0, 6, 6, 6, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.True(result);
        Assert.NotNull(placement);
        Assert.True(placement.SmallestMargin >= 0);
        Assert.True(placement.SupportRatio >= 0.75);
    }

    /// <summary>
    /// اگر آیتم از مرز SubBin بیرون بزند باید False برگردد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemCollidesWithBoundary()
    {
        var item = new Item(10, 1, 1);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }

    /// <summary>
    /// اگر آیتم در هیچ حالتی جا نشود باید لیست نقاط خالی برگردد
    /// </summary>
    [Fact]
    public void GetPoints_ShouldReturnEmpty_WhenItemDoesNotFit()
    {
        var item = new Item(10, 10, 1);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var points = PlacementFeasibilityChecker.GetPoints(subBin, item, 0.75);

        Assert.Empty(points);
    }

    /// <summary>
    /// اگر سطح پشتیبانی کافی نباشد، GetPoints باید نقطه‌ی جایگزین بازگرداند
    /// </summary>
    [Fact]
    public void GetPoints_ShouldFallback_WhenSupportAreaNotEnough()
    {
        var item = new Item(5, 5, 1);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var points = PlacementFeasibilityChecker.GetPoints(subBin, item, 0.9);

        Assert.Single(points);
        Assert.Equal(new Vector3(0, 0, 0), points[0]);
    }

    /// <summary>
    /// تست محاسبه‌ی سطح پشتیبانی در حالتی که آیتم بخشی روی کف قرار دارد
    /// </summary>
    [Fact]
    public void ComputeSupportArea_ShouldBePartial_WhenItemOverhangs()
    {
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var placedBox = new PlacedBox() { X = 3, Y = 0, Z = 0, L = 5, W = 2, H = 1 };

        var result = InvokeComputeSupportArea(subBin, placedBox);

        Assert.Equal(4, result);
    }

    /// <summary>
    /// اگر آیتم چندین اورینتیشن معتبر داشته باشد باید یکی از آن‌ها انتخاب شود
    /// </summary>
    [Fact]
    public void Execute_ShouldSelectCorrectOrientation_WhenMultipleOrientationsPossible()
    {
        var item = new Item(2, 3, 4);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.True(result);
        Assert.NotNull(placement);
        Assert.Contains(placement!.Orientation, item.GetOrientations());
    }

    /// <summary>
    /// اگر کوچکترین Margin برابر صفر باشد هم باید Placement معتبر باشد
    /// </summary>
    [Fact]
    public void Execute_ShouldAllowPlacement_WhenMarginEqualsZero()
    {
        var item = new Item(5, 5, 5);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.True(result);
        Assert.NotNull(placement);
        Assert.True(Math.Abs(placement!.SmallestMargin) < 1e-5);
    }

    /// <summary>
    /// باید مقادیر Custom Margin در SubBin رعایت شوند
    /// </summary>
    [Fact]
    public void Execute_ShouldRespectCustomMargins()
    {
        var item = new Item(2, 2, 2);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 2, 2, 1, 1, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.True(result);
        Assert.NotNull(placement);
        Assert.True(placement!.SmallestMargin >= 0);
    }

    /// <summary>
    /// اگر آیتم کاملاً داخل SubBin قرار گیرد سطح پشتیبانی باید کامل باشد
    /// </summary>
    [Fact]
    public void ComputeSupportArea_ShouldBeFull_WhenItemCompletelyInside()
    {
        var sb = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var pb = new PlacedBox { X = 1, Y = 1, Z = 0, L = 3, W = 3, H = 1 };

        var result = InvokeComputeSupportArea(sb, pb);

        Assert.Equal(9, result);
    }

    /// <summary>
    /// اگر هیچ همپوشانی وجود نداشته باشد سطح پشتیبانی صفر است
    /// </summary>
    [Fact]
    public void ComputeSupportArea_ShouldBeZero_WhenNoOverlap()
    {
        var sb = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);
        var pb = new PlacedBox { X = 10, Y = 10, Z = 0, L = 2, W = 2, H = 1 };

        var result = InvokeComputeSupportArea(sb, pb);

        Assert.Equal(0, result);
    }

    private static int InvokeComputeSupportArea(SubBin sb, PlacedBox pb)
    {
        var method = typeof(PlacementFeasibilityChecker)
            .GetMethod("ComputeSupportArea", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;

        return (int)method.Invoke(null, new object[] { sb, pb })!;
    }

    /// <summary>
    /// اگر طول آیتم از طول SubBin بیشتر باشد باید قرار نگیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemExceedsXBoundary()
    {
        var item = new Item(6, 1, 1);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }

    /// <summary>
    /// اگر عرض آیتم از عرض SubBin بیشتر باشد باید قرار نگیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemExceedsYBoundary()
    {
        var item = new Item(1, 6, 1);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }

    /// <summary>
    /// اگر ارتفاع آیتم از ارتفاع SubBin بیشتر باشد باید قرار نگیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldReturnFalse_WhenItemExceedsZBoundary()
    {
        var item = new Item(1, 1, 6);
        var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }

    /// <summary>
    /// اگر SmallestMargin منفی شود یعنی آیتم بیرون زده و نباید قرار گیرد
    /// </summary>
    [Fact]
    public void Execute_ShouldContinue_WhenSmallestMarginNegative()
    {
        var item = new Item(3, 3, 3);
        var subBin = new SubBin(0, 0, 0, 3, 2, 3, 0, 0, 0, 0, 0);

        var result = _checker.Execute(item, subBin, out var placement);

        Assert.False(result);
        Assert.Null(placement);
    }



    /// <summary>
    /// تست قرارگیری آیتم روی آیتم دیگر (Stacking) با رعایت SupportRatio ≥ λ
    /// </summary>
    [Fact]
    public void Execute_ShouldAllowStacking_WhenSupportRatioIsSatisfied()
    {
        var baseSubBin = new SubBin(0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0);

        var bottomItem = new Item(10, 10, 2);
        var result1 = _checker.Execute(bottomItem, baseSubBin, out var placement1);

        Assert.True(result1);
        Assert.NotNull(placement1);

        var topSubBin = new SubBin(0, 0, 2, 10, 10, 8, 0, 0, 0, 0, 0);

        var topItem = new Item(5, 5, 2);
        var result2 = _checker.Execute(topItem, topSubBin, out var placement2);

        Assert.True(result2);
        Assert.NotNull(placement2);
        Assert.True(placement2!.SupportRatio >= 0.75);
    }
}