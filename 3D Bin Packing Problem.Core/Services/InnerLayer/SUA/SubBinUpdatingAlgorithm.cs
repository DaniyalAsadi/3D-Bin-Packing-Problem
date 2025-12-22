using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

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
            x: placement.Position.X,
            y: placement.Position.Y,
            z: placement.Position.Z,
            l: placement.Orientation.Length,
            w: placement.Orientation.Width,
            h: placement.Orientation.Height
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
        return !(box.X + box.L <= sb.Position.X ||
                 box.X >= sb.Position.X + sb.Size.Length ||
                 box.Y + box.W <= sb.Position.Y ||
                 box.Y >= sb.Position.Y + sb.Size.Width ||
                 box.Z + box.H <= sb.Position.Z ||
                 box.Z >= sb.Position.Z + sb.Size.Height);
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

        var sx1 = sb.Position.X;
        var sx2 = sb.Position.X + sb.Size.Length;
        var sy1 = sb.Position.Y;
        var sy2 = sb.Position.Y + sb.Size.Width;
        var sz1 = sb.Position.Z;
        var sz2 = sb.Position.Z + sb.Size.Height;

        // ۱. Right sub-bin
        if (ix2 < sx2)
        {
            result.Add(new SubBin(
                new Vector3(ix2, sy1, sz1),
                new Dimensions(sx2 - ix2, sb.Size.Width, sb.Size.Height),
                Left: 0, Right: 0, Back: 0, Front: 0
            ));
        }

        // ۲. Front sub-bin
        if (iy2 < sy2)
        {
            result.Add(new SubBin(
                new Vector3(sx1, iy2, sz1),
                new Dimensions(sb.Size.Length, sy2 - iy2, sb.Size.Height),
                Left: 0, Right: 0, Back: 0, Front: 0
            ));
        }

        // ۳. Top sub-bin
        if (iz2 < sz2)
        {
            result.Add(new SubBin(
                new Vector3(sx1, sy1, iz2),
                new Dimensions(sb.Size.Length, sb.Size.Width, sz2 - iz2),
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
        if (Math.Abs(a.Position.Y - b.Position.Y) < Eps &&
            Math.Abs(a.Position.Z - b.Position.Z) < Eps &&
            Math.Abs(a.Size.Width - b.Size.Width) < Eps &&
            Math.Abs(a.Size.Height - b.Size.Height) < Eps)
        {
            if (a.Position.X + a.Size.Length < b.Position.X + Eps && b.Position.X < a.Position.X + a.Size.Length + Eps)
            {
                result = new SubBin(
                    a.Position with { X = Math.Min(a.Position.X, b.Position.X) },
                    new Dimensions(Math.Max(a.Position.X + a.Size.Length, b.Position.X + b.Size.Length) - Math.Min(a.Position.X, b.Position.X),
                    a.Size.Width,
                    a.Size.Height),
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
            if (b.Position.X + b.Size.Length < a.Position.X + Eps && a.Position.X < b.Position.X + b.Size.Length + Eps)
            {
                result = new SubBin(
                    a.Position with { X = Math.Min(a.Position.X, b.Position.X) },
                    new Dimensions(Math.Max(a.Position.X + a.Size.Length, b.Position.X + b.Size.Length) - Math.Min(a.Position.X, b.Position.X),
                    a.Size.Width,
                    a.Size.Height),
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
        }

        // ادغام در جهت Y
        if (Math.Abs(a.Position.X - b.Position.X) < Eps &&
            Math.Abs(a.Position.Z - b.Position.Z) < Eps &&
            Math.Abs(a.Size.Length - b.Size.Length) < Eps &&
            Math.Abs(a.Size.Height - b.Size.Height) < Eps)
        {
            if (a.Position.Y + a.Size.Width < b.Position.Y + Eps && b.Position.Y < a.Position.Y + a.Size.Width + Eps ||
                b.Position.Y + b.Size.Width < a.Position.Y + Eps && a.Position.Y < b.Position.Y + b.Size.Width + Eps)
            {
                result = new SubBin(
                    a.Position with { Y = Math.Min(a.Position.Y, b.Position.Y) },
                    new Dimensions(a.Size.Length,
                    Math.Max(a.Position.Y + a.Size.Width, b.Position.Y + b.Size.Width) - Math.Min(a.Position.Y, b.Position.Y),
                    a.Size.Height),
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
        }

        // ادغام در جهت Z
        if (Math.Abs(a.Position.X - b.Position.X) < Eps &&
            Math.Abs(a.Position.Y - b.Position.Y) < Eps &&
            Math.Abs(a.Size.Length - b.Size.Length) < Eps &&
            Math.Abs(a.Size.Width - b.Size.Width) < Eps)
        {
            if (a.Position.Z + a.Size.Height < b.Position.Z + Eps && b.Position.Z < a.Position.Z + a.Size.Height + Eps ||
                b.Position.Z + b.Size.Height < a.Position.Z + Eps && a.Position.Z < b.Position.Z + b.Size.Height + Eps)
            {
                result = new SubBin(
                    a.Position with { Z = Math.Min(a.Position.Z, b.Position.Z) },
                    new Dimensions(a.Size.Length,
                    a.Size.Width,
                    Math.Max(a.Position.Z + a.Size.Height, b.Position.Z + b.Size.Height) - Math.Min(a.Position.Z, b.Position.Z)),
                    Left: 0, Right: 0, Back: 0, Front: 0
                );
                return true;
            }
        }

        return false;
    }

    private static bool IsContained(SubBin small, SubBin big)
    {
        return small.Position.X >= big.Position.X - Eps &&
               small.Position.Y >= big.Position.Y - Eps &&
               small.Position.Z >= big.Position.Z - Eps &&
               small.Position.X + small.Size.Length <= big.Position.X + big.Size.Length + Eps &&
               small.Position.Y + small.Size.Width <= big.Position.Y + big.Size.Width + Eps &&
               small.Position.Z + small.Size.Height <= big.Position.Z + big.Size.Height + Eps;
    }
}