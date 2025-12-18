namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Represents the dimensions and coordinates of a placed item for feasibility checks.
/// </summary>
public readonly struct PlacedBox(int x, int y, int z, int l, int w, int h)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;
    public int L { get; } = l; // Length
    public int W { get; } = w; // Width
    public int H { get; } = h; // Height
}