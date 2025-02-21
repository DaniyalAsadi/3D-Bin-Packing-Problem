using _3D_Bin_Packing_Problem.Models;
using static _3D_Bin_Packing_Problem.Program;

namespace _3D_Bin_Packing_Problem.CrossOvers;

public class SingleCrossOver : ICrossOver
{
    private readonly Random _random;
    public SingleCrossOver()
    {
        _random = new Random();
    }
    public (Chromosome, Chromosome) CrossOver(Chromosome parent1, Chromosome parent2)
    {
        int crossOverPoint = _random.Next(1, parent1.Genes.Count);

        var child1Genes = new List<Gene>();
        var child2Genes = new List<Gene>();

        child1Genes.AddRange(parent1.Genes.Take(crossOverPoint));
        child1Genes.AddRange(parent2.Genes.Skip(crossOverPoint));

        child2Genes.AddRange(parent2.Genes.Take(crossOverPoint));
        child2Genes.AddRange(parent1.Genes.Skip(crossOverPoint));

        var child1 = new Chromosome { Genes = child1Genes };
        var child2 = new Chromosome { Genes = child2Genes };

        return (child1, child2);
    }
}
