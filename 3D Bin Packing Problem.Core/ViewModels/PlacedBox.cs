namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Represents the dimensions and coordinates of a placed item for feasibility checks.
/// </summary>
public readonly struct PlacedBox
{
    /// <summary>
    /// Represents the dimensions and coordinates of a placed item for feasibility checks.
    /// </summary>
    public PlacedBox(
        int x,
        int y,
        int z,
        int l,
        int w,
        int h)
    {
        X = x;
        Y = y;
        Z = z;
        L = l;
        W = w;
        H = h;
    }

    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int L { get; } // Length
    public int W { get; } // Width
    public int H { get; } // Height
}