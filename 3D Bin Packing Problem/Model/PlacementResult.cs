namespace _3D_Bin_Packing_Problem.Model;

public class PlacementResult
{
    public Product Product { get; set; }
    public SubBox SubBox { get; set; }
    public int OrientationIndex { get; set; } // یکی از 6 حالت چرخش
}