using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Services;
using System.Numerics;
using System.Text;

namespace _3D_Bin_Packing_Problem.Model;

public class Chromosome
{
    public double Fitness { get; set; }
    public BoxPlacement Placement { get; set; }
    private Random _random = new();
    public Chromosome(List<Product> products, List<Box> availableBoxes)
    {
        availableBoxes = availableBoxes.Where(x => x.Volume > products.Sum(y => y.Volume)).ToList();

        int random = _random.Next(0, availableBoxes.Count);

        var selectedBoxes = availableBoxes[random].Clone();
        Placement = new BoxPlacement(selectedBoxes);

        foreach (var product in products)
        {
            Placement.PlaceProduct(product);
        }

        Fitness = FitnessCalculator.CalculateFitness(this);
    }
    public override string ToString()
    {
        return Placement.ToString();
    }
}


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
