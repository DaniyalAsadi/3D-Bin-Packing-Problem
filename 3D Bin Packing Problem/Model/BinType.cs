namespace _3D_Bin_Packing_Problem.Model;

public record BinType
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;

    public double Cost { get; set; }

    public void SetCost(double cost)
    {
        Cost = cost;
    }

    public static implicit operator SubBin(BinType binType)
    {
        return new SubBin(binType);
    }
}