namespace _3D_Bin_Packing_Problem.Model;

public class Bin
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;
    public required double Cost { get; set; }
    public List<Item> PackedItems { get; set; } = [];
    public Bin Clone()
    {
        return new Bin()
        {
            Length = Length,
            Width = Width,
            Height = Height,
            Cost = Cost
        };
    }
    public static implicit operator SubBin(Bin bin)
    {
        return new SubBin(bin);
    }
}