namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Represents the outcome of a fitness evaluation including the resulting packing layout and score.
/// </summary>
public class FitnessResultViewModel
{
    public PackingResultsViewModel PackingResults { get; set; } = new();
    public double Fitness { get; set; }
}
