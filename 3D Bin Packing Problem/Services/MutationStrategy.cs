using System.Numerics;
using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services;

public static class MutationStrategy
{
    private static readonly Random _random = new Random();
    public static void PositionShiftMutation(Chromosome chromosome)
    {
        if (chromosome.Placement.PlacedProducts.Count == 0) return;

        var index = _random.Next(0, chromosome.Placement.PlacedProducts.Count);
        var productPlacement = chromosome.Placement.PlacedProducts[index];

        var xMiddlePosition = _random.Next(productPlacement.Product.Width / 2, (chromosome.Placement.Box.Width - productPlacement.Product.Width / 2) + 1);
        var yMiddlePosition = _random.Next(productPlacement.Product.Height / 2, (chromosome.Placement.Box.Height - productPlacement.Product.Height / 2) + 1);
        var zMiddlePosition = _random.Next(productPlacement.Product.Length / 2, (chromosome.Placement.Box.Length - productPlacement.Product.Length / 2) + 1);

        productPlacement.Middle = new Vector3(xMiddlePosition, yMiddlePosition, zMiddlePosition);
        productPlacement.PositionNodes = productPlacement.Product.ToVector3(productPlacement.Middle);
        chromosome.Fitness = FitnessCalculator.CalculateFitness(chromosome);
    }
    public static void SwapMutation(Chromosome chromosome)
    {
        if (chromosome.Placement.PlacedProducts.Count < 2) return;

        var index1 = _random.Next(0, chromosome.Placement.PlacedProducts.Count);
        var index2 = _random.Next(0, chromosome.Placement.PlacedProducts.Count);

        (chromosome.Placement.PlacedProducts[index1], chromosome.Placement.PlacedProducts[index2]) =
            (chromosome.Placement.PlacedProducts[index2], chromosome.Placement.PlacedProducts[index1]);
    }
    public static void ScrambleMutation(Chromosome chromosome)
    {
        if (chromosome.Placement.PlacedProducts.Count < 2) return;

        int start = _random.Next(0, chromosome.Placement.PlacedProducts.Count - 1);
        int end = _random.Next(start + 1, chromosome.Placement.PlacedProducts.Count);

        var subList = chromosome.Placement.PlacedProducts.GetRange(start, end - start);
        subList = subList.OrderBy(_ => _random.Next()).ToList();

        for (int i = 0; i < subList.Count; i++)
        {
            chromosome.Placement.PlacedProducts[start + i] = subList[i];
        }
    }
    public static void GaussianMutation(Chromosome chromosome, double sigma = 0.1)
    {
        if (chromosome.Placement.PlacedProducts.Count == 0) return;

        var index = _random.Next(0, chromosome.Placement.PlacedProducts.Count);
        var productPlacement = chromosome.Placement.PlacedProducts[index];

        double GaussianRandom() => _random.NextDouble() * sigma * 2 - sigma; // مقدار تصادفی گاوسی

        var newMiddle = new Vector3(
            (float)(productPlacement.Middle.X + GaussianRandom()),
            (float)(productPlacement.Middle.Y + GaussianRandom()),
            (float)(productPlacement.Middle.Z + GaussianRandom())
        );

        productPlacement.Middle = newMiddle;
        productPlacement.PositionNodes = productPlacement.Product.ToVector3(newMiddle);
    }
    public static void OrientationMutation(Chromosome chromosome)
    {
        if (chromosome.Placement.PlacedProducts.Count == 0) return;

        var index = _random.Next(0, chromosome.Placement.PlacedProducts.Count);
        var productPlacement = chromosome.Placement.PlacedProducts[index];

        var orientations = new List<Vector3>
        {
            new(productPlacement.Middle.X, productPlacement.Middle.Z, productPlacement.Middle.Y), // Swap Y-Z
            new(productPlacement.Middle.Z, productPlacement.Middle.Y, productPlacement.Middle.X), // Swap X-Z
            new(productPlacement.Middle.Y, productPlacement.Middle.X, productPlacement.Middle.Z)  // Swap X-Y
        };

        productPlacement.Middle = orientations[_random.Next(orientations.Count)];
        productPlacement.PositionNodes = productPlacement.Product.ToVector3(productPlacement.Middle);
    }
}