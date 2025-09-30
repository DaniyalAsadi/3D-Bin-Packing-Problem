namespace _3D_Bin_Packing_Problem.Model;

public class BinType
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;
    public required double Cost { get; set; }
    public List<Item> PackedItems { get; set; } = [];
    public BinType Clone()
    {
        return new BinType()
        {
            Length = Length,
            Width = Width,
            Height = Height,
            Cost = Cost
        };
    }
    public static implicit operator SubBin(BinType binType)
    {
        return new SubBin(binType);
    }
}