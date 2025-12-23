using Ardalis.GuardClauses;

namespace _3D_Bin_Packing_Problem.Core.Models;

public readonly record struct Dimensions(int Length, int Width, int Height)
{
    public int Length { get; } = Guard.Against.NegativeOrZero(Length);
    public int Width { get; } = Guard.Against.NegativeOrZero(Width);
    public int Height { get; } = Guard.Against.NegativeOrZero(Height);
    public int Volume => Length * Width * Height;
}