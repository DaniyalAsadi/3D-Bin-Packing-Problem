using System.Numerics;
using System.Text;

namespace _3D_Bin_Packing_Problem.Model;

public class BoxPlacement(Box box)
{
    private readonly Random _random = new Random();
    public Box Box { get; set; } = box;
    public List<ProductPlacement> PlacedProducts { get; set; } = [];

    public void PlaceProduct(Product product)
    {
        var xMiddlePosition = _random.Next(product.Width / 2, Box.Width - product.Width / 2);
        var yMiddlePosition = _random.Next(product.Height / 2, Box.Height - product.Height / 2);
        var zMiddlePosition = _random.Next(product.Length / 2, Box.Length - product.Length / 2);

        var middle = new Vector3(xMiddlePosition, yMiddlePosition, zMiddlePosition);

        var productPlacement = new ProductPlacement(product, middle);
        PlacedProducts.Add(productPlacement);
    }
    public void PlaceProduct(List<ProductPlacement> productPlacements)
    {
        PlacedProducts.AddRange(productPlacements);
    }
    public override string ToString()
    {
       StringBuilder builder = new StringBuilder();
        foreach (var product in PlacedProducts)
        {
            builder.AppendLine(product.ToString());
        }
        return builder.ToString();
    }
}
