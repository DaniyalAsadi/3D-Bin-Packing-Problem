namespace _3D_Bin_Packing_Problem.Model;

/// <summary>
/// Describes a bin's dimensional properties and derived cost used within packing evaluations.
/// </summary>
public class BinType
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;
    // میزان تأثیر شکل بر هزینه (۰.۰۵ = تأثیر کم، ۰.۲ = تأثیر متوسط)
    private const double ShapePenaltyFactor = 1000;

    public double Cost
    {
        get
        {
            // میانگین و انحراف معیار اضلاع
            var mean = (Length + Width + Height) / 3.0;
            var variance =
                Math.Pow(Length - mean, 2) +
                Math.Pow(Width - mean, 2) +
                Math.Pow(Height - mean, 2);
            var stdDev = Math.Sqrt(variance / 3.0);

            // مقدار نرمال‌شده انحراف شکل
            var shapePenalty = stdDev / mean; // 0 برای مکعب کامل، ~0.3 برای خیلی کشیده

            // هزینه نهایی: حجم با وزن بسیار بالا + پاداش جزئی برای مکعب بودن
            return Volume * (1 + ShapePenaltyFactor * shapePenalty);
        }
    }
    public static implicit operator SubBin(BinType binType)
    {
        return new SubBin(binType);
    }
    public BinType Clone()
    {
        return new BinType()
        {
            Height = Height,
            Length = Length,
            Width = Width
        };
    }
    /// <summary>
    /// نمایش اطلاعات Bin به صورت خوانا برای دیباگ و لاگ‌ها
    /// </summary>
    public override string ToString()
    {
        return $"Bin [L×W×H=({Length}×{Width}×{Height}), Vol={Volume}, Cost={Cost:F2}]";
    }
}
