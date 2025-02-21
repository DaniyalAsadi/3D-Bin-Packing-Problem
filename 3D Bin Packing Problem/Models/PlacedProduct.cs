namespace _3D_Bin_Packing_Problem.Models;

public class PlacedProduct
{
    public PlacedProduct(Gene gene, int x, int y, int z)
    {
        Gene = gene;
        X = x;
        Y = y;
        Z = z;
    }

    public Gene Gene { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public override string ToString()
    {
        return $"Product {Gene.Product.Id} ({Length}x{Height}x{Width}) Placed At ({X},{Y},{Z}) ({Gene.Orientation})";
    }
}
