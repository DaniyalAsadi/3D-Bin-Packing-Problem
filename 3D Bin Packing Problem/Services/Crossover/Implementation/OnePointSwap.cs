using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Crossover.Implementation;
internal class OnePointSwap : ICrossoverOperator
{
    private static readonly Random Random = new Random();

    public (Chromosome, Chromosome) Crossover(Chromosome c1, Chromosome c2)
    {
        if (c1 == null || c2 == null)
            throw new ArgumentNullException("Chromosomes must not be null");

        if (c1.Count == 0 || c2.Count == 0)
            throw new ArgumentException("Chromosomes must not be empty");

        // pick a crossover point in gene-space
        int crossoverPoint = Random.Next(0, Math.Min(c1.Count, c2.Count));

        // clone parents into children
        var child1Genes = c1.GeneSequences
            .Select(gs => new GeneSequence(gs.Box.Clone()))
            .ToList();

        var child2Genes = c2.GeneSequences
            .Select(gs => new GeneSequence(gs.Box.Clone()))
            .ToList();

        // map crossover point -> seqIndex and geneIndex
        int seqIndex = crossoverPoint / 3;
        int geneIndex = crossoverPoint % 3;

        // swap single gene between children
        (child1Genes[seqIndex][geneIndex], child2Genes[seqIndex][geneIndex]) = (child2Genes[seqIndex][geneIndex], child1Genes[seqIndex][geneIndex]);

        return (new Chromosome(child1Genes), new Chromosome(child2Genes));
    }
}
