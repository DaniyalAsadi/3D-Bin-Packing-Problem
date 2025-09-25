namespace _3D_Bin_Packing_Problem.Model;

public record SubBin(
    int X,
    int Y,
    int Z,
    int Length,
    int Width,
    int Height,
    int Back = 0,
    int Left = 0,
    int Front = 0,
    int Right = 0)
{
    public int Volume => Length * Width * Height;

    public SubBin(Bin bin) : this(
        0,
        0,
        0,
        bin.Length,
        bin.Width,
        bin.Height,
        0,
        0,
        0,
        0)
    {

    }
    // برای خوانایی بیشتر یک ToString
    public override string ToString()
        => $"Pos=({X},{Y},{Z}), Size=({Length}×{Width}×{Height}), " +
           $"Margins [B:{Back}, L:{Left}, F:{Front}, R:{Right}]";
}
