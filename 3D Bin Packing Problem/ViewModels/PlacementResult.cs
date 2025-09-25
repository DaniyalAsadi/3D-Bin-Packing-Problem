using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.ViewModels;

public class PlacementResult
{
    public Item Item { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Orientation { get; set; } // یکی از 6 حالت چرخش
    public double SmallestMargin { get; set; }
    public double SupportRatio { get; set; }
}