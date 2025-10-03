namespace _3D_Bin_Packing_Problem.ViewModels;

public class FitnessResultViewModel
{
    public PackingResultsViewModel PackingResults { get; set; } = new();
    public double Fitness { get; set; }
}
