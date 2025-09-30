namespace _3D_Bin_Packing_Problem.Model;

public record SubBin(
    int X,
    int Y,
    int Z,
    int Length,
    int Width,
    int Height,
    int Back,
    int Left,
    int Front,
    int Right,
    int Top)
{
    public int Volume => Length * Width * Height;

    public SubBin(BinType binType) : this(
        0,
        0,
        0,
        binType.Length,
        binType.Width,
        binType.Height,
        0,
        0,
        0,
        0,
        0)
    {

    }
    // برای خوانایی بیشتر یک ToString
    public override string ToString()
        => $"Pos=({X},{Y},{Z}), Size=({Length}×{Width}×{Height}), " +
           $"Margins [B:{Back}, L:{Left}, F:{Front}, R:{Right}] T:{Top}";


    public int GetMinimumDimension()
    {
        return Math.Min(Length, Math.Min(Width, Height));

    }
}
