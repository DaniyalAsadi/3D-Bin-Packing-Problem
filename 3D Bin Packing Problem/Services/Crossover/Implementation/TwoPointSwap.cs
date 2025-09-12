using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Crossover.Implementation;

internal class TwoPointSwap : ICrossoverOperator
{
    private static readonly Random Random = new Random(3);
    public (Chromosome, Chromosome) Crossover(Chromosome c1, Chromosome c2)
    {
        var n = c1.Count;
        if (n < 2) return (new Chromosome(c1), new Chromosome(c2));
        var pts = Enumerable.Range(0, n).OrderBy(x => Random.Next()).Take(2).OrderBy(x => x).ToArray();
        int i = pts[0], j = pts[1];
        var chromosome1 = new Chromosome(c1);
        var chromosome2 = new Chromosome(c2);
        for (var k = i; k <= j; k++)
            (chromosome1[k], chromosome2[k]) = (chromosome2[k], chromosome1[k]);
        return (chromosome1, chromosome2);
    }
}