using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;

/// <summary>
/// Updates the collection of sub-bins after placing an item by splitting and merging affected regions.
/// </summary>
public class SubBinUpdatingAlgorithm : ISubBinUpdatingAlgorithm
{
    public List<SubBin> Execute(List<SubBin> subBinList, PlacementResult? placement)
    {
        var newSubBinList = new List<SubBin>();
        if (placement == null)
            return subBinList; // آیتم جا نشد → بدون تغییر

        // --- خط 3: تقسیم ---
        foreach (var subBin in subBinList.ToList())
        {
            if (!HasOverlap(subBin, placement)) continue;
            var divided = Divide(subBin, placement);
            newSubBinList.AddRange(divided);
            subBinList.Remove(subBin);
        }

        // --- خط 10: ادغام ---
        foreach (var sb in subBinList.ToList())
        {
            foreach (var nsb in newSubBinList.ToList())
            {
                if (IsContained(nsb, sb))
                {
                    newSubBinList.Remove(nsb);
                }
                else if (IsContained(sb, nsb))
                {
                    subBinList.Remove(sb);
                }
            }
        }

        // --- خط 21: افزودن ---
        subBinList.AddRange(newSubBinList);

        return subBinList;
    }

    private static bool HasOverlap(SubBin sb, PlacementResult placement)
    {
        var ix = (int)placement.Position.X;
        var iy = (int)placement.Position.Y;
        var iz = (int)placement.Position.Z;

        var il = (int)placement.Orientation.X;
        var iw = (int)placement.Orientation.Y;
        var ih = (int)placement.Orientation.Z;

        var overlapX = !(ix + il <= sb.X || ix >= sb.X + sb.Length);
        var overlapY = !(iy + iw <= sb.Y || iy >= sb.Y + sb.Width);
        var overlapZ = !(iz + ih <= sb.Z || iz >= sb.Z + sb.Height);

        return overlapX && overlapY && overlapZ;
    }

    private static List<SubBin> Divide(SubBin sb, PlacementResult placement)
    {
        var result = new List<SubBin>();

        var ix = (int)placement.Position.X;
        var iy = (int)placement.Position.Y;
        var iz = (int)placement.Position.Z;
        var il = (int)placement.Orientation.X;
        var iw = (int)placement.Orientation.Y;
        var ih = (int)placement.Orientation.Z;


        // Right
        if (ix + il < sb.X + sb.Length)
        {
            result.Add(sb with
            {
                X = ix + il,
                Length = sb.X + sb.Length - (ix + il),
                Left = sb.Left + (ix + il - sb.X)
            });
        }


        // Front
        if (iy + iw < sb.Y + sb.Width)
        {
            result.Add(sb with
            {
                Y = iy + iw,
                Width = sb.Y + sb.Width - (iy + iw),
                Back = sb.Back + (iy + iw - sb.Y)
            });
        }


        // Top
        if (iz + ih < sb.Z + sb.Height)
        {
            result.Add(sb with
            {
                Z = iz + ih,
                Height = sb.Z + sb.Height - (iz + ih)
            });
        }

        return result;
    }


    private static bool IsContained(SubBin a, SubBin b)
    {
        return a.X >= b.X &&
               a.Y >= b.Y &&
               a.Z >= b.Z &&
               a.X + a.Length <= b.X + b.Length &&
               a.Y + a.Width <= b.Y + b.Width &&
               a.Z + a.Height <= b.Z + b.Height;
    }
}
