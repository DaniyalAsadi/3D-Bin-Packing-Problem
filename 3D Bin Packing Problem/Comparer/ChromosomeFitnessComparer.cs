using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.FitnessCalculator;

public class ChromosomeFitnessComparer(IFitness fitness) : IComparer<Chromosome>
{
    public int Compare(Chromosome? x, Chromosome? y)
    {
        if (x == null || y == null) return 0;

        var fx = fitness.Evaluate(x);
        var fy = fitness.Evaluate(y);

        return fx.CompareTo(fy);
    }
}