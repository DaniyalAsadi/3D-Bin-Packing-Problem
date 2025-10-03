using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.ViewModels;

public class PlacementResult
{
    public PlacementResult(Item item, Vector3 position, Vector3 orientation, double smallestMargin, double supportRatio)
    {
        Item = item;
        Position = position;
        Orientation = orientation;
        SmallestMargin = smallestMargin;
        SupportRatio = supportRatio;
    }

    public Item Item { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Orientation { get; set; } // یکی از 6 حالت چرخش
    public double SmallestMargin { get; set; }
    public double SupportRatio { get; set; }
}