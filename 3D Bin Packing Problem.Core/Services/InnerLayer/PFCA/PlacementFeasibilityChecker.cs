using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Extensions;
using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;

/// <summary>
/// Evaluates whether an item can be feasibly placed within a sub-bin while respecting margins and support constraints.
/// </summary>
public class PlacementFeasibilityChecker : IPlacementFeasibilityChecker
{
    private const float Eps = AppConstants.Tolerance;
    private readonly double _lambda = SettingsManager.Current.Genetic.SupportThreshold;

    public static PlacementResult? LastPlacement { get; private set; }

    public bool Execute(BinType binType, Item item, SubBin subBin, out PlacementResult? result)
    {
        result = null;
        LastPlacement = null;

        if (subBin.Volume < item.Volume)
            return false;

        var bestMargin = double.PositiveInfinity;
        PlacementResult? bestResult = null;

        var orientations = item.GetOrientations().ToList();

        foreach (var orientation in orientations)
        {
            var L = orientation.Length;
            var W = orientation.Width;
            var H = orientation.Height; // فقط برای خوانایی

            var keyPoints = GetKeyPoints(subBin, L, W, _lambda);

            foreach (var pos in keyPoints)
            {
                // فقط روی کف SubBin اجازه قرارگیری داریم
                if (Math.Abs(pos.Z - subBin.Position.Z) > Eps)
                    continue;

                var placedBox = new PlacedBox(
                    x: pos.X,
                    y: pos.Y,
                    z: pos.Z,
                    l: L,
                    w: W,
                    h: H
                );

                // بررسی مرزهای مجاز (با احتساب حاشیه‌ها)
                if (placedBox.X < subBin.Position.X - subBin.Left ||
                    placedBox.Y < subBin.Position.Y - subBin.Back ||
                    placedBox.X + L > subBin.Position.X + subBin.Size.Length + subBin.Right ||
                    placedBox.Y + W > subBin.Position.Y + subBin.Size.Width + subBin.Front ||
                    placedBox.Z + H > subBin.Position.Z + subBin.Size.Height)
                    continue;

                // محاسبه حاشیه‌ها (فاصله تا نزدیک‌ترین دیوار مجاز)
                var marginLeft = placedBox.X - (subBin.Position.X - subBin.Left);
                var marginRight = (subBin.Position.X + subBin.Size.Length + subBin.Right) - (placedBox.X + L);
                var marginBack = placedBox.Y - (subBin.Position.Y - subBin.Back);
                var marginFront = (subBin.Position.Y + subBin.Size.Width + subBin.Front) - (placedBox.Y + W);
                var marginTop = (subBin.Position.Z + subBin.Size.Height) - (placedBox.Z + H);

                var margins = new[] { marginLeft, marginRight, marginBack, marginFront, marginTop };
                var smallestMargin = margins.Min();

                if (smallestMargin < -Eps)
                    continue; // خارج از محدوده مجاز

                // محاسبه نسبت تکیه‌گاه (فقط روی کف اصلی SubBin)
                var supportArea = ComputeSupportArea(subBin, placedBox);
                var itemBaseArea = (double)L * W;
                var supportRatio = itemBaseArea > 0 ? supportArea / itemBaseArea : 0.0;

                if (supportRatio < _lambda - Eps)
                    continue;

                // بهترین موقعیت بر اساس بیشترین حاشیه کوچک‌ترین (Stable + Compact)
                if (!(smallestMargin < bestMargin - Eps)) continue;
                bestMargin = smallestMargin;
                bestResult = new PlacementResult(
                    Item: item,
                    BinType: binType,
                    Position: pos,
                    Orientation: orientation,
                    SmallestMargin: smallestMargin,
                    SupportRatio: supportRatio
                );
            }
        }

        result = bestResult;
        LastPlacement = bestResult;
        return bestResult != null;
    }

    /// <summary>
    /// تولید ۵ نقطه کلیدی استاندارد (Extreme Points + λ-constraint)
    /// کاملاً سازگار با حاشیه‌ها (Left/Back/Right/Front)
    /// </summary>
    private static IReadOnlyList<Vector3> GetKeyPoints(SubBin sb, float L, float W, double lambda)
    {
        lambda = Math.Clamp(lambda, 0.0, 1.0);

        var xMin = sb.Position.X - sb.Left;
        var xMax = sb.Position.X + sb.Size.Length + sb.Right - L;
        var yMin = sb.Position.Y - sb.Back;
        var yMax = sb.Position.Y + sb.Size.Width + sb.Front - W;

        if (xMin > xMax || yMin > yMax)
            return Array.Empty<Vector3>();

        var itemArea = (long)L * W;
        var requiredArea = (long)Math.Ceiling(lambda * itemArea);

        var coreX1 = sb.Position.X;
        var coreX2 = sb.Position.X + sb.Size.Length;
        var coreY1 = sb.Position.Y;
        var coreY2 = sb.Position.Y + sb.Size.Width;

        var points = new List<Vector3>();

        // Point 1: تا حد امکان عقب و چپ (Back-Left)
        points.Add(new Vector3(xMin, yMin, sb.Position.Z));

        // Point 2: چسبیده به پشت، ولی کمی جلو برای تأمین λ
        if (lambda > 0 && requiredArea > 0)
        {
            float maxOverlapX = Math.Min(L, sb.Size.Length);
            if (maxOverlapX > 0)
            {
                float neededY = (requiredArea + maxOverlapX - 1) / maxOverlapX; // ceil division
                float y2 = coreY1 + neededY - W;
                y2 = Math.Max(y2, yMin);
                y2 = Math.Min(y2, yMax);
                points.Add(new Vector3(xMin, y2, sb.Position.Z));
            }
        }

        // Point 3: چسبیده به چپ، ولی کمی راست برای تأمین λ
        if (lambda > 0 && requiredArea > 0)
        {
            float maxOverlapY = Math.Min(W, sb.Size.Width);
            if (maxOverlapY > 0)
            {
                float neededX = (requiredArea + maxOverlapY - 1) / maxOverlapY;
                float x3 = coreX1 + neededX - L;
                x3 = Math.Max(x3, xMin);
                x3 = Math.Min(x3, xMax);
                points.Add(new Vector3(x3, yMin, sb.Position.Z));
            }
        }

        // Point 4 & 5: گوشه اصلی کف (Core Bottom-Left) — معمولاً بهترین نقطه برای پایداری
        points.Add(new Vector3(sb.Position.X, sb.Position.Y, sb.Position.Z));

        // حذف تکراری‌ها با دقت بالا
        var unique = new List<Vector3>();
        foreach (var p in points)
        {
            bool exists = unique.Any(q =>
                Math.Abs(q.X - p.X) < Eps &&
                Math.Abs(q.Y - p.Y) < Eps);

            if (!exists)
                unique.Add(p);
        }

        return unique;
    }

    /// <summary>
    /// محاسبه دقیق مساحت تکیه‌گاه روی کف اصلی SubBin
    /// فقط وقتی Z برابر باشد و تقاطع با بخش اصلی (Core) داشته باشد
    /// </summary>
    private static float ComputeSupportArea(SubBin sb, PlacedBox box)
    {
        // فقط روی کف SubBin ساپورت داریم
        if (Math.Abs(box.Z - sb.Position.Z) > Eps)
            return 0f;

        var coreX1 = sb.Position.X;
        var coreX2 = sb.Position.X + sb.Size.Length;
        var coreY1 = sb.Position.Y;
        var coreY2 = sb.Position.Y + sb.Size.Width;

        var boxX1 = box.X;
        var boxX2 = box.X + box.L;
        var boxY1 = box.Y;
        var boxY2 = box.Y + box.W;

        var ox1 = Math.Max(coreX1, boxX1);
        var ox2 = Math.Min(coreX2, boxX2);
        var oy1 = Math.Max(coreY1, boxY1);
        var oy2 = Math.Min(coreY2, boxY2);

        if (ox2 <= ox1 || oy2 <= oy1)
            return 0f;

        return (ox2 - ox1) * (oy2 - oy1);
    }
}