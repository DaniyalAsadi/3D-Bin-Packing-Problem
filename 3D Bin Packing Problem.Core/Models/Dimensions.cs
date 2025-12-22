using Ardalis.GuardClauses;

namespace _3D_Bin_Packing_Problem.Core.Models;

public record Dimensions(float Length, float Width, float Height)
{
    public float Length { get; } = Guard.Against.NegativeOrZero(Length);
    public float Width { get; } = Guard.Against.NegativeOrZero(Width);
    public float Height { get; } = Guard.Against.NegativeOrZero(Height);
    public float Volume => Length * Width * Height;
}