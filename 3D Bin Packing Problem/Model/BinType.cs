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
    private Func<double>? _valueFunc;

    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }

    /// <summary>
    /// حجم Bin فعلی
    /// </summary>
    public int Volume => Length * Width * Height;

    
    /// <summary>
    /// هزینه Bin بر اساس فرمول مقاله Alvarez-Valdés et al. (2013)
    /// Ct = 10000 × (1.2Vt − 0.2LWH) / (LWH)
    /// </summary>
    public double Cost
    {
        get => _valueFunc != null ? _valueFunc() : 0.0;
    }


    public Func<double> CostFunc
    {
        set => _valueFunc = value;
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

