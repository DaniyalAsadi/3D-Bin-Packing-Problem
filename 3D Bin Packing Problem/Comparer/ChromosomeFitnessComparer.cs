using _3D_Bin_Packing_Problem.Model;

public class ChromosomeFitnessComparer : IComparer<Chromosome>
{
    public int Compare(Chromosome? x, Chromosome? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        var fx = x.Fitness ?? double.MinValue;
        var fy = y.Fitness ?? double.MinValue;

        // Descending: higher fitness first
        return fy.CompareTo(fx);
    }
}
