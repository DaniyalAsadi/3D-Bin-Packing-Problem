namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Represents the dimensions and coordinates of a placed item for feasibility checks.
/// </summary>
public readonly struct PlacedBox
{
    /// <summary>
    /// Represents the dimensions and coordinates of a placed item for feasibility checks.
    /// </summary>
    public PlacedBox(float x, float y, float z, float l, float w, float h)
    {
        X = x;
        Y = y;
        Z = z;
        L = l;
        W = w;
        H = h;
    }

    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public float L { get; } // Length
    public float W { get; } // Width
    public float H { get; } // Height
}