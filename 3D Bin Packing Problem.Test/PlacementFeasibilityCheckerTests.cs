using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;

namespace BinPacking.Tests
{
    public class PlacementFeasibilityCheckerTests
    {
        private readonly PlacementFeasibilityChecker _checker = new();

        [Fact]
        public void Execute_ShouldReturnFalse_WhenItemVolumeGreaterThanSubBin()
        {
            var item = new Item(10, 10, 10); // حجم = 1000
            var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0); // حجم = 125

            var result = _checker.Execute(item, subBin, out var placement);

            Assert.False(result);
            Assert.Null(placement);
        }

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

        [Fact]
        public void Execute_ShouldReturnFalse_WhenSupportRatioTooLow()
        {
            var item = new Item(5, 5, 1);
            var subBin = new SubBin(0, 0, 0, 6, 2, 1, 0, 0, 0, 0, 0); // کف خیلی باریک

            var result = _checker.Execute(item, subBin, out var placement);

            Assert.False(result);
            Assert.Null(placement);
        }

        [Fact]
        public void GetPoints_ShouldReturnFiveUniquePoints()
        {
            var item = new Item(2, 2, 1);
            var subBin = new SubBin(0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0);

            var points = PlacementFeasibilityChecker.GetPoints(subBin, item, 0.75);

            Assert.NotEmpty(points);
            Assert.True(points.Count <= 5);
            Assert.All(points, p => Assert.True(p.Z == 0)); // همه روی کف قرار دارند
        }

        [Fact]
        public void Execute_ShouldChooseBestMarginPlacement()
        {
            var item = new Item(2, 2, 2);
            var subBin = new SubBin(0, 0, 0, 6, 6, 6, 0, 0, 0, 0, 0);

            var result = _checker.Execute(item, subBin, out var placement);

            Assert.True(result);
            Assert.NotNull(placement);
            Assert.True(placement!.SmallestMargin > 0);
            Assert.True(placement.SupportRatio >= 0.75);
        }
    }
}
