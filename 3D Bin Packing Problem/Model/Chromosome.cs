using _3D_Bin_Packing_Problem.Services;

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
