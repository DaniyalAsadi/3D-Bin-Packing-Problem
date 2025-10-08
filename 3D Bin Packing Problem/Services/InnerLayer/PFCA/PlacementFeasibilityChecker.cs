using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.ViewModels;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.PFCA;

/// <summary>
/// Evaluates whether an item can be feasibly placed within a sub-bin while respecting margins and support constraints.
/// </summary>
public class PlacementFeasibilityChecker : IPlacementFeasibilityChecker
{
    private const float EPS = 1e-5f;
    private const double Lambda = 0.75;
    public static PlacementResult? LastPlacement { get; set; }


    public bool Execute(Item item, SubBin subBin, out PlacementResult? result)
    {
        if (subBin.Volume < item.Volume)
        {
            result = null;
            return false;
        }
        var bestMargin = double.PositiveInfinity;
        PlacementResult? bestResult = null;

        var keyPoints = GetPoints(subBin, item, Lambda);
        var orientations = item.GetOrientations().ToList();

        foreach (var pos in keyPoints)
        {
            foreach (var orientation in orientations)
            {
                // موقعیت آیتم بعد از چرخش
                var placedBox = new PlacedBox()
                {
                    X = (int)pos.X,
                    Y = (int)pos.Y,
                    Z = (int)pos.Z,
                    L = (int)orientation.X,
                    W = (int)orientation.Y,
                    H = (int)orientation.Z
                };

                // بررسی برخورد با مرزهای SubBin
                if (placedBox.X + placedBox.L > subBin.X + subBin.Length + subBin.Right ||
                    placedBox.Y + placedBox.W > subBin.Y + subBin.Width + subBin.Front ||
                    placedBox.Z + placedBox.H > subBin.Z + subBin.Height)
                {
                    continue; // این اورینتیشن نمی‌گنجه
                }

                // --- محاسبه margins ---
                var marginLeft = placedBox.X - (subBin.X - subBin.Left);
                var marginRight = subBin.X + subBin.Length + subBin.Right - (placedBox.X + placedBox.L);
                var marginBack = placedBox.Y - (subBin.Y - subBin.Back);
                var marginFront = subBin.Y + subBin.Width + subBin.Front - (placedBox.Y + placedBox.W);
                var marginTop = subBin.Z + subBin.Height - (placedBox.Z + placedBox.H);

                var margins = new[] { marginLeft, marginRight, marginBack, marginFront, marginTop };
                var smallestMargin = margins.Min();

                if (smallestMargin < -EPS)
                    continue; // یعنی برخورد کرده

                // --- بررسی نسبت تکیه‌گاه (support ratio) ---
                var supportArea = ComputeSupportArea(subBin, placedBox);
                var itemArea = placedBox.L * placedBox.W;
                var supportRatio = (double)supportArea / itemArea;

                if (supportRatio < Lambda) continue; // شرط ساپورت برقرار نیست

                // --- انتخاب بهترین ---
                if (!(smallestMargin < bestMargin)) continue;
                bestMargin = smallestMargin;
                bestResult = new PlacementResult(
                    Item: item,
                    Position: pos,
                    Orientation: orientation,
                    SmallestMargin: smallestMargin,
                    SupportRatio: supportRatio);
            }
        }

        result = bestResult;
        LastPlacement = bestResult;
        return bestResult != null;
    }

    /// <summary>
    /// محاسبه مساحت تکیه‌گاه آیتم روی کف SubBin
    /// (برای ساده‌سازی اینجا فرض می‌کنیم کف کامل ساپورت هست)
    /// </summary>
    private static int ComputeSupportArea(SubBin sb, PlacedBox placedBox)
    {
        // ساده‌سازی: تقاطع بین کف subBin و قاعده آیتم
        int x1 = Math.Max(sb.X, placedBox.X);
        int y1 = Math.Max(sb.Y, placedBox.Y);
        int x2 = Math.Min(sb.X + sb.Length, placedBox.X + placedBox.L);
        int y2 = Math.Min(sb.Y + sb.Width, placedBox.Y + placedBox.W);

        if (x2 <= x1 || y2 <= y1)
            return 0;

        return (x2 - x1) * (y2 - y1);
    }
    /// <summary>
    /// تولید نقاط 1..5 برای یک آیتم با اورینتیشن فعلی (Length×Width روی کف) و نسبت تکیه‌گاه λ.
    /// اگر Point2/Point4 دست‌یافتنی نباشند، به ترتیب برابر Point1/Point3 می‌شوند.
    /// خروجی به ترتیب 1→5 و بدون تکراری است.
    /// </summary>
    public static IReadOnlyList<Vector3> GetPoints(SubBin sb, Item item, double lambda)
    {
        lambda = Math.Clamp(lambda, 0.0, 1.0);

        // محدوده‌ی مجاز مبدأ آیتم (MER + Extension) روی کف z=sb.Z
        var xMin = sb.X - sb.Left;
        var xMax = sb.X + sb.Length + sb.Right - item.Length;
        var yMin = sb.Y - sb.Back;
        var yMax = sb.Y + sb.Width + sb.Front - item.Width;

        if (xMin > xMax || yMin > yMax)
            return Array.Empty<Vector3>(); // آیتم با این اورینتیشن داخل این ساب‌بن جا نمی‌شود

        // مساحت لازم روی بخش «خاکستری» (سطح دارای تکیه‌گاه)
        var itemArea = (long)item.Length * item.Width;
        var needArea = lambda * itemArea;

        // --- شاخه Back: نقاط 1 و 2 (x ثابت، y تغییر می‌کند) ---
        // بیشینه‌ی هم‌پوشانی در x با بخش خاکستری وقتی x=sb.X:
        var supportX = Math.Min(item.Length, sb.Length);
        // بیشینه‌ی هم‌پوشانی ممکن در y:
        var maxYOverlap = Math.Min(item.Width, sb.Width);

        var y1 = yMin; // "تا حد امکان به سمت پشت"
        int y2;
        if (supportX == 0 || needArea > (long)supportX * maxYOverlap)
        {
            // Point 2 وجود ندارد؛ همان Point 1
            y2 = y1;
        }
        else
        {
            // کمترین جابه‌جایی به سمت جلو که λ را تامین کند
            var needY = needArea / supportX;            // طول هم‌پوشانی لازم در y
            var y2Ideal = sb.Y + needY - item.Width;    // فرمول از هندسه‌ی هم‌پوشانی
            y2 = (int)Math.Ceiling(y2Ideal);               // Ceil تا نسبت کم نشود
            y2 = Math.Clamp(y2, yMin, yMax);
        }

        var p1 = new Vector3(sb.X, y1, sb.Z);
        var p2 = new Vector3(sb.X, y2, sb.Z);

        // --- شاخه Left: نقاط 3 و 4 (y ثابت، x تغییر می‌کند) ---
        var supportY = Math.Min(item.Width, sb.Width);
        var maxXOverlap = Math.Min(item.Length, sb.Length);

        var x3 = xMin; // "تا حد امکان به سمت چپ"
        int x4;
        if (supportY == 0 || needArea > (long)maxXOverlap * supportY)
        {
            // Point 4 وجود ندارد؛ همان Point 3
            x4 = x3;
        }
        else
        {
            var needX = needArea / supportY;           // طول هم‌پوشانی لازم در x
            var x4Ideal = sb.X + needX - item.Length;  // کمترین جابه‌جایی به چپ که λ را تامین کند
            x4 = (int)Math.Ceiling(x4Ideal);
            x4 = Math.Clamp(x4, xMin, xMax);
        }

        var p3 = new Vector3(x3, sb.Y, sb.Z);
        var p4 = new Vector3(x4, sb.Y, sb.Z);

        var p5 = new Vector3(sb.X, sb.Y, sb.Z);

        // حذف تکراری‌ها با حفظ ترتیب 1→5
        var ordered = new[] { p1, p2, p3, p4, p5 };
        var unique = new List<Vector3>(5);
        foreach (var kp in ordered)
            if (!unique.Any(q =>
                    Math.Abs(q.X - kp.X) < EPS &&
                    Math.Abs(q.Y - kp.Y) < EPS &&
                    Math.Abs(q.Z - kp.Z) < EPS))
            {
                unique.Add(kp);
            }

        return unique;
    }
}

/// <summary>
/// Represents the dimensions and coordinates of a placed item for feasibility checks.
/// </summary>
public readonly struct PlacedBox
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }
    public int L { get; init; } // Length
    public int W { get; init; } // Width
    public int H { get; init; } // Height
}
