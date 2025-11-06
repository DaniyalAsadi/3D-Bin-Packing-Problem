using _3D_Bin_Packing_Problem.Core.Model;
using System;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover.Implementation;

/// <summary>
/// Replaces a randomly selected sequence in one chromosome with the corresponding sequence from another.
/// </summary>
public class UniformCrossover : ICrossoverOperator
{
    private static readonly Random Random = new Random();

    public (Chromosome, Chromosome) Crossover(Chromosome c1, Chromosome c2)
    {
        if (c1 == null || c2 == null)
            throw new ArgumentNullException(nameof(c1));

        if (c1.Count == 0 || c2.Count == 0)
            throw new ArgumentException("Chromosomes must not be empty");

        var chromosome1 = c1.Clone();
        var chromosome2 = c2.Clone();

        for (var i = 0; i < c1.Count; i++)
        {
            if (!(Random.NextDouble() < 0.5)) continue;
            // Randomly decide which parent to take the gene from
            chromosome1[i] = c2[i];
            chromosome2[i] = c1[i];
        }

        return (chromosome1, chromosome2);
    }
}
