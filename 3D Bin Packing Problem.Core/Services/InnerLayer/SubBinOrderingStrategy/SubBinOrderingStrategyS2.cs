using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

/// <summary>
/// Orders sub-bins based on their contact area with existing surfaces to promote stable placements.
/// </summary>
public class SubBinOrderingStrategyS2 : ISubBinOrderingStrategy
{
    private int ComputeTouchArea(Item item, SubBin sb)
    {
        int touchArea = 0;

        // تماس با کف (Z = 0)
        if (sb.Z == 0)
            touchArea += item.Length * item.Width;

        // تماس با دیوار پشت (Back = 0)
        if (sb.Back == 0)
            touchArea += item.Width * item.Height;

        // تماس با دیوار چپ (Left = 0)
        if (sb.Left == 0)
            touchArea += item.Length * item.Height;

        // تماس با دیوار جلو (Front = 0)
        if (sb.Front == 0)
            touchArea += item.Width * item.Height;

        // تماس با دیوار راست (Right = 0)
        if (sb.Right == 0)
            touchArea += item.Length * item.Height;

        return touchArea;
    }

    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderByDescending(sb => ComputeTouchArea(item, sb))
            .ThenByDescending(sb => sb.X)
            .ThenByDescending(sb => sb.Y)
            .ThenByDescending(sb => sb.Z);
    }
}
