using _3D_Bin_Packing_Problem.Core.Model;
using System;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover.Implementation;

/// <summary>
/// Performs crossover by swapping two independently chosen genes between parent chromosomes.
/// </summary>
public class MultiPointCrossover : ICrossoverOperator
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

        for (var i = 0; i < 3; i++)  // Perform crossover at 3 random points
        {
            var crossoverPoint1 = Random.Next(0, c1.Count);
            var crossoverPoint2 = Random.Next(0, c2.Count);

            var seqIndex1 = crossoverPoint1 / 3;
            var seqIndex2 = crossoverPoint2 / 3;

            (chromosome1[seqIndex1], chromosome2[seqIndex2]) = (chromosome2[seqIndex2], chromosome1[seqIndex1]);
        }

        return (chromosome1, chromosome2);
    }
}