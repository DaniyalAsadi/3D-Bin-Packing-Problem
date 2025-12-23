using _3D_Bin_Packing_Problem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins based on their contact area with existing surfaces to promote stable placements.
/// </summary>
public class SubBinOrderingStrategyS2 : ISubBinOrderingStrategy
{
    private double ComputeTouchArea(Item item, SubBin sb)
    {
        double touchArea = 0;

        // تماس با کف (Z = 0)
        if (sb.Position.Z == 0)
            touchArea += item.Dimensions.Length * item.Dimensions.Width;

        // تماس با دیواره پشت (Back = 0)
        if (sb.Back == 0)
            touchArea += item.Dimensions.Width * item.Dimensions.Height;

        // تماس با دیواره جلو (Front = 0)
        if (sb.Front == 0)
            touchArea += item.Dimensions.Width * item.Dimensions.Height;

        // تماس با دیواره چپ (Left = 0)
        if (sb.Left == 0)
            touchArea += item.Dimensions.Length * item.Dimensions.Height;

        // تماس با دیواره راست (Right = 0)
        if (sb.Right == 0)
            touchArea += item.Dimensions.Length * item.Dimensions.Height;

        return touchArea;
    }


    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            // مرتب‌سازی بر اساس بیشترین ناحیه تماس (Touch Area)
            .OrderByDescending(sb => ComputeTouchArea(item, sb))
            // معیارهای تساوی: X، Y، Z به ترتیب نزولی
            .ThenByDescending(sb => sb.Position.X)
            .ThenByDescending(sb => sb.Position.Y)
            .ThenByDescending(sb => sb.Position.Z);
    }
}
