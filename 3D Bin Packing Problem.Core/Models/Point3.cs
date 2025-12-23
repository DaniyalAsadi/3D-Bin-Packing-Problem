namespace _3D_Bin_Packing_Problem.Core.Models;

public readonly record struct Point3(int X, int Y, int Z)
{
    public int X { get; } = X;
    public int Y { get; } = Y;
    public int Z { get; } = Z;
}