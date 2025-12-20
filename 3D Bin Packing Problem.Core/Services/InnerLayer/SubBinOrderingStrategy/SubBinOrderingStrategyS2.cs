using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins based on their contact area with existing surfaces to promote stable placements.
/// </summary>
public class SubBinOrderingStrategyS2 : ISubBinOrderingStrategy
{
    private const double Eps = AppConstants.Tolerance;

    private double ComputeTouchArea(Item item, SubBin sb)
    {
        double touchArea = 0;

        // تماس با کف (Z = 0)
        if (Math.Abs(sb.Position.Z) < Eps)
            touchArea += item.Dimensions.Length * item.Dimensions.Width;

        // تماس با دیواره پشت (Back = 0)
        if (Math.Abs(sb.Back) < Eps)
            touchArea += item.Dimensions.Width * item.Dimensions.Height;

        // تماس با دیواره جلو (Front = 0)
        if (Math.Abs(sb.Front) < Eps)
            touchArea += item.Dimensions.Width * item.Dimensions.Height;

        // تماس با دیواره چپ (Left = 0)
        if (Math.Abs(sb.Left) < Eps)
            touchArea += item.Dimensions.Length * item.Dimensions.Height;

        // تماس با دیواره راست (Right = 0)
        if (Math.Abs(sb.Right) < Eps)
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
