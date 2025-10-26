using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Helpers;

/// <summary>
/// Provides helper functionality for computing bounds on bin dimensions based on a collection of items.
/// </summary>
public static class BinDimensionBounds
{
    /// <summary>
    /// Represents the minimum and maximum extents for each dimension of the bin.
    /// </summary>
    public sealed record Bounds
    {
        /// <summary>
        /// Represents the minimum and maximum extents for each dimension of the bin.
        /// </summary>
        public Bounds(
            int minLength,
            int minWidth,
            int minHeight,
            int maxLength,
            int maxWidth,
            int maxHeight)
        {
            MinLength = minLength;
            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxLength = maxLength;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }

        public int MinLength { get; }
        public int MinWidth { get; }
        public int MinHeight { get; }
        public int MaxLength { get; }
        public int MaxWidth { get; }
        public int MaxHeight { get; }

    }

    public static Bounds Compute(IReadOnlyCollection<Item> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("items must not be null or empty.", nameof(items));

        // d1 <= d2 <= d3 برای هر آیتم
        var sortedTriples = items
            .Select(it =>
            {
                var dims = new[] { it.Length, it.Width, it.Height };
                Array.Sort(dims); // ascending
                return (d1: dims[0], d2: dims[1], d3: dims[2]);
            })
            .ToList();

        // حداقل‌ها (B1,B2,B3) بر اساس شرط چرخش
        var minL = sortedTriples.Max(t => t.d1); // B1
        var minW = sortedTriples.Max(t => t.d2); // B2
        var minH = sortedTriples.Max(t => t.d3); // B3

        // جمع‌های ستونی برای کران بالایی ردیفی
        var sumD1 = sortedTriples.Sum(t => t.d1);
        var sumD2 = sortedTriples.Sum(t => t.d2);
        var sumD3 = sortedTriples.Sum(t => t.d3);

        // حداکثرها: چیدن ردیفی روی هر ستون + ایمن‌سازی با حداقل‌ها
        var maxL = Math.Max(sumD1, minL);
        var maxW = Math.Max(sumD2, minW);
        var maxH = Math.Max(sumD3, minH);

        // (اختیاری) کمی تلرانس بده تا Genetic diversity بهتر بشه
        // مثلا: maxL += (int)Math.Ceiling(minL * 0.1); و مشابه برای W,H

        return new Bounds(minL, minW, minH, maxL, maxW, maxH);
    }
}

