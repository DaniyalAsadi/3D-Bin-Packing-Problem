using _3D_Bin_Packing_Problem.Core.Models;
using System;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation.Implementation;

/// <summary>
/// Applies mutation by swapping two randomly selected genes within a chromosome clone.
/// </summary>
public class TwoPointMutation : IMutationOperator
{
    private static readonly Random Random = new Random();
    public Chromosome Mutate(Chromosome chromosome)
    {
        var mutated = chromosome.Clone(); // clone
        var crossoverPoint1 = Random.Next(0, chromosome.Count);
        var crossoverPoint2 = Random.Next(crossoverPoint1, chromosome.Count);
        var crm = chromosome.Clone();
        for (var i = crossoverPoint1; i < crossoverPoint2; i++)
        {
            crm[i].ApplyRandomRotation();
        }
        return mutated;
    }
}
