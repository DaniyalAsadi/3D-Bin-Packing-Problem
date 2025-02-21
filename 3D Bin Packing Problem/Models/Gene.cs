namespace _3D_Bin_Packing_Problem.Models;

public class Gene
{
    public Gene(Product product, Orientation orientation)
    {
        Product = product;
        Orientation = orientation;
    }
    public Product Product { get; set; }
    public Orientation Orientation { get; set; }
    public override string ToString() => $"Gene {Product.ToString()} ({Orientation})";
}
