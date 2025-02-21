namespace _3D_Bin_Packing_Problem.Models;

public record FitnessFunctionWeights
{
    public FitnessFunctionWeights()
    {
    }
    public FitnessFunctionWeights(double basePackingFactor, double maxPackingFactor, double baseCostFactor, double minCostFactor)
    {
        BasePackingFactor = basePackingFactor;
        MaxPackingFactor = maxPackingFactor;
        BaseCostFactor = baseCostFactor;
        MinCostFactor = minCostFactor;
    }
    public double BasePackingFactor { get; init; } = 0.5;
    public double MaxPackingFactor { get; init; } = 2.0;

    public double BaseCostFactor { get; init; } = 2.0;
    public double MinCostFactor { get; init; } = 0.5;
}
