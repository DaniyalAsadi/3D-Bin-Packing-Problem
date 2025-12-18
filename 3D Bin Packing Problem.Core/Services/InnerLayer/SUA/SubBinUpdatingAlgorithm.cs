using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Model;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SUA;

/// <summary>
/// Updates the collection of sub-bins after placing an item by splitting and merging affected regions.
/// </summary>
public class SubBinUpdatingAlgorithm : ISubBinUpdatingAlgorithm
{
    private const float Eps = AppConstants.Tolerance;

    public List<SubBin> Execute(List<SubBin> currentSubBins, PlacementResult? placement)
    {
        if (placement == null) throw new ArgumentNullException(nameof(placement));

        var placedBox = new PlacedBox(
            x: (int)placement.Position.X,
            y: (int)placement.Position.Y,
            z: (int)placement.Position.Z,
            l: (int)placement.Orientation.X,
            w: (int)placement.Orientation.Y,
            h: (int)placement.Orientation.Z
        );

        var newSubBins = new List<SubBin>();
        var toRemove = new HashSet<SubBin>();

        // مرحله ۱: تقسیم تمام SubBinهایی که با آیتم هم‌پوشانی دارند
        foreach (var sb in currentSubBins)
        {
            if (!HasOverlap(sb, placedBox))
            {
                continue;
            }

            toRemove.Add(sb);
            var fragments = DivideSubBin(sb, placedBox);
            newSubBins.AddRange(fragments);
        }

        // حذف SubBinهای تقسیم‌شده
        foreach (var sb in toRemove)
            currentSubBins.Remove(sb);

        // مرحله ۲: حذف SubBinهای جدید که داخل قبلی‌ها هستند (Containment)
        newSubBins = newSubBins
            .Where(ns => !currentSubBins.Any(os => IsContained(ns, os)))
            .ToList();

        // مرحله ۳: ادغام (Merge) SubBinهای مجاور و هم‌تراز
        var allSubBins = currentSubBins.Concat(newSubBins).ToList();
        var merged = MergeAdjacentSubBins(allSubBins);

        // مرحله ۴: حذف دوباره زیرمجموعه‌ها بعد از ادغام
        var finalList = new List<SubBin>();
        foreach (var sb in merged)
        {
            if (!finalList.Any(big => IsContained(sb, big)))
                finalList.Add(sb);
        }

        return finalList;
    }

    private static bool HasOverlap(SubBin sb, PlacedBox box)
    {
        return !(box.X + box.L <= sb.X ||
                 box.X >= sb.X + sb.Length ||
                 box.Y + box.W <= sb.Y ||
                 box.Y >= sb.Y + sb.Width ||
                 box.Z + box.H <= sb.Z ||
                 box.Z >= sb.Z + sb.Height);
    }

    /// <summary>
    /// تقسیم Guillotine: فقط ۳ قطعه (Right, Front, Top) — حاشیه‌ها صفر می‌شوند
    /// </summary>
    private static List<SubBin> DivideSubBin(SubBin sb, PlacedBox box)
    {
        var result = new List<SubBin>();

        var ix1 = box.X;
        var ix2 = box.X + box.L;
        var iy1 = box.Y;
        var iy2 = box.Y + box.W;
        var iz1 = box.Z;
        var iz2 = box.Z + box.H;

        var sx1 = sb.X;
        var sx2 = sb.X + sb.Length;
        var sy1 = sb.Y;
        var sy2 = sb.Y + sb.Width;
        var sz1 = sb.Z;
        var sz2 = sb.Z + sb.Height;

        // ۱. Right sub-bin
        if (ix2 < sx2)
        {
            result.Add(new SubBin(
                X: ix2,
                Y: sy1,
                Z: sz1,
                Length: sx2 - ix2,
                Width: sb.Width,
                Height: sb.Height,
                Left: 0, Right: 0, Back: 0, Front: 0
            ));
        }

        // ۲. Front sub-bin
        if (iy2 < sy2)
        {
            result.Add(new SubBin(
                X: sx1,
                Y: iy2,
                Z: sz1,
                Length: sb.Length,
                Width: sy2 - iy2,
                Height: sb.Height,
                Left: 0, Right: 0, Back: 0, Front: 0
            ));
        }

        // ۳. Top sub-bin
        if (iz2 < sz2)
        {
            result.Add(new SubBin(
                X: sx1,
                Y: sy1,
                Z: iz2,
                Length: sb.Length,
                Width: sb.Width,
                Height: sz2 - iz2,
                Left: 0, Right: 0, Back: 0, Front: 0
            ));
        }

        return result.Where(s => s.Volume > Eps).ToList();
    }

    /// <summary>
    /// ادغام SubBinهای مجاور در سه جهت (X, Y, Z)
    /// </summary>
    private static List<SubBin> MergeAdjacentSubBins(List<SubBin> subBins)
    {
        var active = new List<SubBin>(subBins);
        var merged = new List<SubBin>();
        var used = new bool[active.Count];

        for (var i = 0; i < active.Count; i++)
        {
            if (used[i]) continue;

            var current = active[i];
            bool changed;

            do
            {
                changed = false;
                for (var j = 0; j < active.Count; j++)
                {
                    if (used[j] || i == j) continue;

                    var other = active[j];

                    if (TryMergeTwo(current, other, out var mergedBox))
                    {
                        current = mergedBox;
                        used[j] = true;
                        changed = true;
                    }
                }
            } while (changed);

            merged.Add(current);
            used[i] = true;
        }

        return merged;
    }

    /// <summary>
    /// تلاش برای ادغام دو SubBin (فقط اگر دقیقاً مجاور و هم‌تراز باشند)
    /// </summary>
    private static bool TryMergeTwo(SubBin a, SubBin b, [NotNullWhen(true)] out SubBin? result)
    {
        result = null;

        // ادغام در جهت X
        if (Math.Abs(a.Y - b.Y) < Eps &&
            Math.Abs(a.Z - b.Z) < Eps &&
            Math.Abs(a.Width - b.Width) < Eps &&
            Math.Abs(a.Height - b.Height) < Eps)
        {
            if (a.X + a.Length < b.X + Eps && b.X < a.X + a.Length + Eps)
            {
                result = new SubBin(
                    X: Math.Min(a.X, b.X),
                    Y: a.Y,
                    Z: a.Z,
                    Length: Math.Max(a.X + a.Length, b.X + b.Length) - Math.Min(a.X, b.X),
                    Width: a.Width,
                    Height: a.Height,
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
            if (b.X + b.Length < a.X + Eps && a.X < b.X + b.Length + Eps)
            {
                result = new SubBin(
                    X: Math.Min(a.X, b.X),
                    Y: a.Y,
                    Z: a.Z,
                    Length: Math.Max(a.X + a.Length, b.X + b.Length) - Math.Min(a.X, b.X),
                    Width: a.Width,
                    Height: a.Height,
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
        }

        // ادغام در جهت Y
        if (Math.Abs(a.X - b.X) < Eps &&
            Math.Abs(a.Z - b.Z) < Eps &&
            Math.Abs(a.Length - b.Length) < Eps &&
            Math.Abs(a.Height - b.Height) < Eps)
        {
            if (a.Y + a.Width < b.Y + Eps && b.Y < a.Y + a.Width + Eps ||
                b.Y + b.Width < a.Y + Eps && a.Y < b.Y + b.Width + Eps)
            {
                result = new SubBin(
                    X: a.X,
                    Y: Math.Min(a.Y, b.Y),
                    Z: a.Z,
                    Length: a.Length,
                    Width: Math.Max(a.Y + a.Width, b.Y + b.Width) - Math.Min(a.Y, b.Y),
                    Height: a.Height,
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
        }

        // ادغام در جهت Z
        if (Math.Abs(a.X - b.X) < Eps &&
            Math.Abs(a.Y - b.Y) < Eps &&
            Math.Abs(a.Length - b.Length) < Eps &&
            Math.Abs(a.Width - b.Width) < Eps)
        {
            if (a.Z + a.Height < b.Z + Eps && b.Z < a.Z + a.Height + Eps ||
                b.Z + b.Height < a.Z + Eps && a.Z < b.Z + b.Height + Eps)
            {
                result = new SubBin(
                    X: a.X,
                    Y: a.Y,
                    Z: Math.Min(a.Z, b.Z),
                    Length: a.Length,
                    Width: a.Width,
                    Height: Math.Max(a.Z + a.Height, b.Z + b.Height) - Math.Min(a.Z, b.Z),
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
        }

        return false;
    }

    private static bool IsContained(SubBin small, SubBin big)
    {
        return small.X >= big.X - Eps &&
               small.Y >= big.Y - Eps &&
               small.Z >= big.Z - Eps &&
               small.X + small.Length <= big.X + big.Length + Eps &&
               small.Y + small.Width <= big.Y + big.Width + Eps &&
               small.Z + small.Height <= big.Z + big.Height + Eps;
    }
}