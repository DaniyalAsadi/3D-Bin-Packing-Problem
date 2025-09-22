namespace _3D_Bin_Packing_Problem.Model;

public class PlacementResult
{
    public Item Item { get; set; }
    public SubBin SubBin { get; set; }
    public (int L, int W, int H) Orientation { get; set; } // یکی از 6 حالت چرخش
    public (int L, int W, int H) KeyPoint { get; set; }
}