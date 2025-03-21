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

        int random = _random.Next(0, availableBoxes.Count);

        var selectedBoxes = availableBoxes[random].Clone();
        Placement = new BoxPlacement(selectedBoxes);

        foreach (var product in products)
        {
            Placement.PlaceProduct(product);
        }

        Fitness = FitnessCalculator.CalculateFitness(this);
    }
    public Chromosome(BoxPlacement boxPlacement)
    {
        Placement = boxPlacement;
        Fitness = FitnessCalculator.CalculateFitness(this);

    }
    public override string ToString()
    {
        return Placement.ToString();
    }

    internal void Mutate()
    {
        var random = _random.Next(0, Placement.PlacedProducts.Count);
        var x = _random.Next(-1, 1);
        var y = _random.Next(-1, 1);
        var z = _random.Next(-1, 1);
        Placement.PlacedProducts[random].PositionNodes = 
            Placement.PlacedProducts[random].PositionNodes
            .Select(o => new Vector3(
                o.X + x, 
                o.Y + y, 
                o.Z + z)).ToArray();
        Fitness = FitnessCalculator.CalculateFitness(this);
    }
}
