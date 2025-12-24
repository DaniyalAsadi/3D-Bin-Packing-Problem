using _3D_Bin_Packing_Problem.Core.Configuration;
using _3D_Bin_Packing_Problem.Core.Extensions;
using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PFCA;

/// <summary>
/// Evaluates whether an item can be feasibly placed within a sub-bin while respecting margins and support constraints.
/// </summary>
public class PlacementFeasibilityChecker : IPlacementFeasibilityChecker
{
    private readonly double _lambda = SettingsManager.Current.Genetic.SupportThreshold;

    public bool Execute(
        BinType binType,
        Item item,
        SubBin subBin,
        out PlacementResult? result)
    {
        result = null;

        if (subBin.Volume < item.Volume)
            return false;

        var bestMargin = double.PositiveInfinity;
        PlacementResult? bestResult = null;

        var orientations = item.GetOrientations().ToList();

        foreach (var orientation in orientations)
        {
            var L = orientation.Length;
            var W = orientation.Width;
            var H = orientation.Height;

            var keyPoints = GetKeyPoints(subBin, L, W, _lambda);

            foreach (var pos in keyPoints)
            {
                var placedBox = new PlacedBox(
                    x: pos.X,
                    y: pos.Y,
                    z: pos.Z,
                    l: L,
                    w: W,
                    h: H
                );

                // Boundary checks
                if (placedBox.X < subBin.Position.X - subBin.Left ||
                    placedBox.Y < subBin.Position.Y - subBin.Back ||
                    placedBox.X + L > subBin.Position.X + subBin.Size.Length + subBin.Right ||
                    placedBox.Y + W > subBin.Position.Y + subBin.Size.Width + subBin.Front ||
                    placedBox.Z + H > subBin.Position.Z + subBin.Size.Height)
                    continue;

                // Margins
                var marginLeft = placedBox.X - (subBin.Position.X - subBin.Left);
                var marginRight = (subBin.Position.X + subBin.Size.Length + subBin.Right) - (placedBox.X + L);
                var marginBack = placedBox.Y - (subBin.Position.Y - subBin.Back);
                var marginFront = (subBin.Position.Y + subBin.Size.Width + subBin.Front) - (placedBox.Y + W);
                var marginTop = (subBin.Position.Z + subBin.Size.Height) - (placedBox.Z + H);

                var margins = new[] { marginLeft, marginRight, marginBack, marginFront, marginTop };
                var smallestMargin = margins.Min();

                if (smallestMargin < 0)
                    continue;

                // Support ratio
                var supportArea = ComputeSupportArea(subBin, placedBox);
                var itemBaseArea = L * W;
                var supportRatio = supportArea / itemBaseArea;

                if (supportRatio < _lambda)
                    continue;

                // Tight packing: minimize smallest margin
                if (smallestMargin >= bestMargin)
                    continue;

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
        return bestResult != null;
    }

    // ----------------------------------------------------

    private static IReadOnlyList<Point3> GetKeyPoints(SubBin sb, int L, int W, double lambda)
    {
        lambda = Math.Clamp(lambda, 0.0, 1.0);

        var xMin = sb.Position.X - sb.Left;
        var xMax = sb.Position.X + sb.Size.Length + sb.Right - L;
        var yMin = sb.Position.Y - sb.Back;
        var yMax = sb.Position.Y + sb.Size.Width + sb.Front - W;

        if (xMin > xMax || yMin > yMax)
            return Array.Empty<Point3>();

        var itemArea = L * W;
        var requiredArea = (int)Math.Ceiling(lambda * itemArea);

        var coreX1 = sb.Position.X;
        var coreY1 = sb.Position.Y;

        var points = new List<Point3>
        {
            // Back-Left
            new Point3(xMin, yMin, sb.Position.Z),

            // Core
            new Point3(sb.Position.X, sb.Position.Y, sb.Position.Z)
        };

        // Back + λ shift
        if (lambda > 0 && requiredArea > 0)
        {
            var maxOverlapX = Math.Min(L, sb.Size.Length);
            if (maxOverlapX > 0)
            {
                var neededY = (requiredArea + maxOverlapX - 1) / maxOverlapX;
                var y2 = coreY1 + neededY - W;
                y2 = Math.Clamp(y2, yMin, yMax);
                points.Add(new Point3(xMin, y2, sb.Position.Z));
            }
        }

        // Left + λ shift
        if (!(lambda > 0) || requiredArea <= 0)
        {
            return points
                .GroupBy(p => (p.X, p.Y))
                .Select(g => g.First())
                .ToList();
        }

        var maxOverlapY = Math.Min(W, sb.Size.Width);
        if (maxOverlapY <= 0)
        {
            return points
                .GroupBy(p => (p.X, p.Y))
                .Select(g => g.First())
                .ToList();
        }

        var neededX = (requiredArea + maxOverlapY - 1) / maxOverlapY;
        var x3 = coreX1 + neededX - L;
        x3 = Math.Clamp(x3, xMin, xMax);
        points.Add(new Point3(x3, yMin, sb.Position.Z));

        // Remove duplicates
        return points
            .GroupBy(p => (p.X, p.Y))
            .Select(g => g.First())
            .ToList();
    }

    private static double ComputeSupportArea(SubBin sb, PlacedBox box)
    {
        if (box.Z != sb.Position.Z)
            return 0.0;

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
            return 0.0;

        return (double)(ox2 - ox1) * (oy2 - oy1);
    }
}
