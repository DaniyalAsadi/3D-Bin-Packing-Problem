namespace _3D_Bin_Packing_Problem.Model;

public class Box
{
    public required int Length { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public int Volume => Width * Height * Length;
    public required decimal Price { get; set; }

    public Box Clone()
    {
        return new Box()
        {
            Length = Height,
            Width = Width,
            Height = Height,
            Price = Price
        };
    }

}