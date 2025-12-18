using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover.Implementation;

/// <summary>
/// Swaps entire gene sequences between chromosomes at randomly chosen boundaries.
/// </summary>
public class TwoPointCrossover : ICrossoverOperator
{
    private static readonly Random Random = new Random();
    public IEnumerable<Chromosome> Crossover(Chromosome c1, Chromosome c2)
    {
        if (c1 == null || c2 == null)
            throw new ArgumentNullException(nameof(c1));

        if (c1.Count == 0 || c2.Count == 0)
            throw new ArgumentException("Chromosomes must not be empty");

        var chromosome1 = c1.Clone();
        var chromosome2 = c2.Clone();

        for (var i = 0; i < 2; i++)
        {
            var crossoverPoint1 = Random.Next(0, c1.Count);
            var crossoverPoint2 = Random.Next(0, c2.Count);

            (chromosome1[crossoverPoint1], chromosome2[crossoverPoint2]) = (chromosome2[crossoverPoint2], chromosome1[crossoverPoint1]);

        }

        return [chromosome1, chromosome2];
    }
}
