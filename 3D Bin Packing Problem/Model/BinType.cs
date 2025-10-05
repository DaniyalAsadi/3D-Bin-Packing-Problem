namespace _3D_Bin_Packing_Problem.Model;

public class BinType
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;
    // میزان تأثیر شکل بر هزینه (۰.۰۵ = تأثیر کم، ۰.۲ = تأثیر متوسط)
    private const double ShapePenaltyFactor = 10;

    public double Cost
    {
        get
        {
            double L = Length;
            double W = Width;
            double H = Height;
            double Vt = Volume;

            // فرمول مقاله:
            // Ct = 10000 * (1.2 * Vt - 0.2 * L * W * H) / (L * W * H)
            var cost = 10000 * ((1.2 * Vt - 0.2 * L * W * H) / (L * W * H));
            return cost;
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