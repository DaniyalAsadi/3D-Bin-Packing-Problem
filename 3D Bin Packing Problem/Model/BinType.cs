namespace _3D_Bin_Packing_Problem.Model;

public record BinType
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;
    public double Cost
    {
        get
        {
            double mean = (Length + Width + Height) / 3.0;
            double variance =
                Math.Pow(Length - mean, 2) +
                Math.Pow(Width - mean, 2) +
                Math.Pow(Height - mean, 2);

            double stdDev = Math.Sqrt(variance / 3.0); // مکعب=۰، مستطیل>۰

            return Volume * (1 + stdDev / mean); // هرچه اختلاف اضلاع بیشتر → هزینه بیشتر
        }
    }


    public static implicit operator SubBin(BinType binType)
    {
        return new SubBin(binType);
    }
}