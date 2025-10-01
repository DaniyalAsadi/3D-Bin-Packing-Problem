using _3D_Bin_Packing_Problem.Model;

public class ChromosomeFitnessComparer : IComparer<Chromosome>
{
    public int Compare(Chromosome? x, Chromosome? y)
    {
        if (x == null || y == null) return 0;

        var fx = x.Fitness;
        var fy = y.Fitness;

        return fx.CompareTo(fy);
    }
}