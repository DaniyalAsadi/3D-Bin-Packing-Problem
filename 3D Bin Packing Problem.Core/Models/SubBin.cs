using System;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Core.Models;

/// <summary>
/// Represents a sub-region within a bin along with its dimensions and boundary margins.
/// </summary>
public record SubBin
{
    public Vector3 Position { get; }
    public Dimensions Size { get; }
    public int Back { get; }
    public int Left { get; }
    public int Front { get; }
    public int Right { get; }

    public float Volume => Size.Volume;

    public SubBin(BinType binType) : this(
        new Vector3(0, 0, 0),
        new Dimensions(binType.InnerDimensions.Length,
        binType.InnerDimensions.Width,
        binType.InnerDimensions.Height))
    { }

    /// <summary>
    /// Represents a sub-region within a bin along with its dimensions and boundary margins.
    /// </summary>
    public SubBin(
        Vector3 position,
        Dimensions size,
        int Back = 0,
        int Left = 0,
        int Front = 0,
        int Right = 0)
    {
        Position = position;
        Size = size;
        this.Back = Back;
        this.Left = Left;
        this.Front = Front;
        this.Right = Right;
    }

    public float GetMinimumDimension() { return Math.Min(Size.Length, Math.Min(Size.Width, Size.Height)); }
}