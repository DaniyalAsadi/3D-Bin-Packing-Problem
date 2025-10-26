using System.Collections.Generic;
using _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Compares chromosomes based on fitness values in descending order.
/// </summary>
public class ChromosomeFitnessComparer : IComparer<Chromosome>
{
    public int Compare(Chromosome? x, Chromosome? y)
    {
        return x switch
        {
            null when y == null => 0,
            null => -1,
            _ => y == null
                ? 1
                :
                // Descending: higher fitness first
                x.Fitness.CompareTo(y.Fitness)
        };
    }
}
