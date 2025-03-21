using _3D_Bin_Packing_Problem.Model;
namespace _3D_Bin_Packing_Problem.Services;
internal class FitnessCalculator
{
    // 1 - Dont Have Overlapping
    // 2 - Fit Inside of Box

    public static double CalculateFitness(Chromosome chromosome)
    {
        double fitness = 0;
        var overlappingMatrix = OverLappingMatrix(chromosome);
        for (int i = 0; i < chromosome.Placement.PlacedProducts.Count; i++)
        {
            for (int j = i + 1; j < chromosome.Placement.PlacedProducts.Count; j++)
            {
                if (overlappingMatrix[i, j])
                {
                    fitness = double.MaxValue;
                }
            }
        }
        return fitness;
    }

    public static bool[,] OverLappingMatrix(Chromosome chromosome)
    {
        bool[,] overlappingMatrix = new bool[chromosome.Placement.PlacedProducts.Count, chromosome.Placement.PlacedProducts.Count];
        for (int i = 0; i < chromosome.Placement.PlacedProducts.Count; i++)
        {
            for (int j = i + 1; j < chromosome.Placement.PlacedProducts.Count; j++)
            {
                overlappingMatrix[i, j] = SAT3D.IsColliding(chromosome.Placement.PlacedProducts[i].PositionNodes, chromosome.Placement.PlacedProducts[j].PositionNodes);
            }
        }
        return overlappingMatrix;
    }
}
