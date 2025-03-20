using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Services;
using System.Numerics;

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
}


public class BoxPlacement(Box box)
{
    private readonly Random _random = new Random();
    public Box Box { get; set; } = box;
    public List<ProductPlacement> PlacedProducts { get; set; } = [];

    public void PlaceProduct(Product product)
    {
        var xMiddlePosition = _random.Next(0, Box.Width);
        var yMiddlePosition = _random.Next(0, Box.Height);
        var zMiddlePosition = _random.Next(0, Box.Length);

        var middle = new Vector3(xMiddlePosition, yMiddlePosition, zMiddlePosition);

        var productPlacement = new ProductPlacement(product, middle);
        PlacedProducts.Add(productPlacement);
    }
}
public class ProductPlacement(Product product, Vector3 middle)
{
    public Product Product { get; set; } = product;
    public Vector3[] PositionNodes { get; set; } = product.ToVector3(middle);
    public Vector3 Middle { get; set; } = middle;

}
