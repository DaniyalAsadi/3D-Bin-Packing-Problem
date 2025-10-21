namespace _3D_Bin_Packing_Problem.Model;

/// <summary>
/// Describes a bin's dimensional properties and derived cost used within packing evaluations.
/// </summary>
public class BinType
{
    // --- Base Reference Bin (from the benchmark definition)
    public static readonly BinType BaseBinType = new BinType
    {
        Length = 2,
        Width = 2,
        Height = 2
    };

    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }

    /// <summary>
    /// حجم Bin فعلی
    /// </summary>
    public int Volume => Length * Width * Height;

    /// <summary>
    /// ضریب جریمه برای شکل‌های غیرمکعبی (فرم غیربهینه)
    /// Shape Penalty Factor = میزان انحراف از شکل مکعب
    /// </summary>
    private const double ShapePenaltyFactor = 0.05; // بر اساس توضیح مقاله: ۰.۰۵ تا ۰.۲

    /// <summary>
    /// هزینه Bin بر اساس فرمول مقاله Alvarez-Valdés et al. (2013)
    /// Ct = 10000 × (1.2Vt − 0.2LWH) / (LWH)
    /// </summary>
    public double Cost
    {
        get
        {
            var baseVolume = (double)(BaseBinType.Length * BaseBinType.Width * BaseBinType.Height);
            var currentVolume = (double)Volume;

            // Cost formula from the paper
            double cost = 10000.0 * ((1.2 * currentVolume - 0.2 * baseVolume) / baseVolume);


            return cost;
        }
    }


    /// <summary>
    /// تبدیل ضمنی به SubBin
    /// </summary>
    public static implicit operator SubBin(BinType binType) => new SubBin(binType);

    /// <summary>
    /// ایجاد نسخه‌ی کپی از BinType
    /// </summary>
    public BinType Clone()
    {
        return new BinType
        {
            Length = Length,
            Width = Width,
            Height = Height
        };
    }

    /// <summary>
    /// نمایش اطلاعات Bin به‌صورت خوانا برای لاگ و دیباگ
    /// </summary>
    public override string ToString()
    {
        return $"Bin [L×W×H=({Length}×{Width}×{Height}), Vol={Volume}, Cost={Cost:F2}]";
    }
}

