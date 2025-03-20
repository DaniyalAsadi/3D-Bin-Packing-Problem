using _3D_Bin_Packing_Problem.Extensions;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Model;

public class ProductPlacement(Product product, Vector3 middle)
{
    public Product Product { get; set; } = product;
    public Vector3[] PositionNodes { get; set; } = product.ToVector3(middle);
    public Vector3 Middle { get; set; } = middle;

    public override string ToString()
    {
        return $"Product: {Product.ToString()}, Middle: {Middle} ";
    }

}
