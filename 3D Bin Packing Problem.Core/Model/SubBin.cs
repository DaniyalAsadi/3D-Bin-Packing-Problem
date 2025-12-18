using System;

namespace _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Represents a sub-region within a bin along with its dimensions and boundary margins.
/// </summary>
public record SubBin(
    int X,
    int Y,
    int Z,
    float Length,
    float Width,
    float Height,
    int Back = 0,
    int Left = 0,
    int Front = 0,
    int Right = 0)
{
    public float Volume => Length * Width * Height;
    public int X { get; } = X;
    public int Y { get; } = Y;
    public int Z { get; } = Z;
    public float Length { get; } = Length;
    public float Width { get; } = Width;
    public float Height { get; } = Height;
    public int Back { get; } = Back;
    public int Left { get; } = Left;
    public int Front { get; } = Front;
    public int Right { get; } = Right;

    public SubBin(BinType binType) : this(
        0,
        0,
        0,
        binType.InnerDimensions.Length,
        binType.InnerDimensions.Width,
        binType.InnerDimensions.Height)
    {
    }

    // برای خوانایی بیشتر یک ToString
    public override string ToString()
        => $"Pos=({X},{Y},{Z}), Size=({Length}×{Width}×{Height}), " +
           $"Margins [B:{Back}, L:{Left}, F:{Front}, R:{Right}]";

    public float GetMinimumDimension()
    {
        return Math.Min(Length, Math.Min(Width, Height));
    }
}
