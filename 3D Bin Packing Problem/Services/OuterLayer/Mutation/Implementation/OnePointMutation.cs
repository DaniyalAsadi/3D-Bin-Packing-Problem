using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.Mutation.Implementation;

/// <summary>
/// Applies mutation by randomly altering a single gene within a chromosome.
/// </summary>
public class OnePointMutation : IMutationOperator
{

    private static readonly Random Random = new Random();
    public Chromosome Mutate(Chromosome chromosome)
    {
        var mutated = chromosome.Clone(); // clone
        var p1 = Random.Next(0, mutated.Count);
        var crossoverPoint1 = Random.Next(0, chromosome.Count);
        // map crossover point -> seqIndex and geneIndex
        var seqIndex1 = crossoverPoint1 / 3;
        var geneIndex1 = crossoverPoint1 % 3;

        mutated[seqIndex1][geneIndex1].SetValue(Random.Next());

        return mutated;
    }
}