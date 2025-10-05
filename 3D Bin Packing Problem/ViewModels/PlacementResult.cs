using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.ViewModels;

public record PlacementResult(
    Item Item,
    Vector3 Position,
    Vector3 Orientation,
    double SmallestMargin,
    double SupportRatio)
{
    public Item Item { get; set; } = Item;
    public Vector3 Position { get; set; } = Position;
    public Vector3 Orientation { get; set; } = Orientation; // یکی از 6 حالت چرخش
    public double SmallestMargin { get; set; } = SmallestMargin;
    public double SupportRatio { get; set; } = SupportRatio;
}