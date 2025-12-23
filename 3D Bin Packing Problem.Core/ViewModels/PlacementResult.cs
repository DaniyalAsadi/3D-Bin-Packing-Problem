using _3D_Bin_Packing_Problem.Core.Models;

namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Describes the placement characteristics of an item within a bin, including position, orientation, and support.
/// </summary>
public record PlacementResult(
    Item Item,
    BinType BinType,
    Point3 Position,
    Dimensions Orientation,
    double SmallestMargin,
    double SupportRatio)
{
    public Item Item { get; set; } = Item;
    public BinType BinType { get; set; } = BinType;
    public Point3 Position { get; set; } = Position;
    public Dimensions Orientation { get; set; } = Orientation; // یکی از 6 حالت چرخش
    public double SmallestMargin { get; set; } = SmallestMargin;
    public double SupportRatio { get; set; } = SupportRatio;
}
