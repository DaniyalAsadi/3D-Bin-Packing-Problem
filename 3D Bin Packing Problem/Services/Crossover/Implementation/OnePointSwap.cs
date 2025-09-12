using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Crossover.Implementation;
internal class OnePointSwap : ICrossoverOperator
{
    private static readonly Random Random = new Random(2);
    public (Chromosome, Chromosome) Crossover(Chromosome c1, Chromosome c2)
    {
        var n = Math.Min(c1.Count, c2.Count);
        if (n == 0) return (new Chromosome(c1), new Chromosome(c2));

        var point = Random.Next(1, n); // crossover after this index

        var child1Genes = c1.Genes.Take(point).Concat(c2.Genes.Skip(point)).ToList();
        var child2Genes = c2.Genes.Take(point).Concat(c1.Genes.Skip(point)).ToList();

        return (new Chromosome(child1Genes), new Chromosome(child2Genes));
    }

}