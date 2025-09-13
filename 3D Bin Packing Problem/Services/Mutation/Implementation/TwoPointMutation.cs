using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Mutation.Implementation;

internal class TwoPointMutation : IMutationOperator
{
    private static readonly Random Random = new Random();
    public Chromosome Mutate(Chromosome chromosome)
    {
        var mutated = chromosome.Clone(); // clone
        var crossoverPoint = Random.Next(0, chromosome.Count);
        var crm = chromosome.Clone();
        for (var i = 0; i < 2; i++)
        {
            // pick a crossover point in gene-space
            var crossoverPoint1 = Random.Next(0, chromosome.Count);
            // map crossover point -> seqIndex and geneIndex
            var seqIndex1 = crossoverPoint1 / 3;
            var geneIndex1 = crossoverPoint1 % 3;


            var seqIndex2 = crossoverPoint1 / 3;
            var geneIndex2 = crossoverPoint % 3;

            // swap single gene between children
            (crm[seqIndex1][geneIndex1], crm[seqIndex2][geneIndex2]) = (crm[seqIndex2][geneIndex2], crm[seqIndex1][geneIndex1]);
        }
        return mutated;
    }
}
