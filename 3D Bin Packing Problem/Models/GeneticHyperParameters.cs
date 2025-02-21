using _3D_Bin_Packing_Problem.CrossOvers;

namespace _3D_Bin_Packing_Problem.Models;

public class GeneticHyperParameters
{
    public GeneticHyperParameters(
        int populationSize,
        int generations,
        double mutationRate)
    {
        PopulationSize = populationSize;
        Generations = generations;
        MutationRate = mutationRate;
    }
    public GeneticHyperParameters(double basePackingFactor, double maxPackingFactor, double baseCostFactor, double minCostFactor)
    {
        FitnessFunctionWeights = new FitnessFunctionWeights(basePackingFactor, maxPackingFactor, baseCostFactor, minCostFactor);

    }
    public int PopulationSize { get; set; } = 30;
    public int Generations { get; set; } = 30;
    public double MutationRate { get; set; } = 0.15;

    public int PerformTournamentSelection { get; set; } = 5;

    public CrossoverType CrossoverType { get; set; } = CrossoverType.Uniform;

    public int BoxSelectionLimit { get; set; } = 3;
    public FitnessFunctionWeights FitnessFunctionWeights { get; set; } = new();

}
