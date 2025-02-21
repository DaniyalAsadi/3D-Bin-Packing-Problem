using static _3D_Bin_Packing_Problem.Program;

namespace _3D_Bin_Packing_Problem.CrossOvers;

public class ArithmeticCrossOver : ICrossOver
{
    public (Chromosome, Chromosome) CrossOver(Chromosome parent1, Chromosome parent2)
    {
        var child1 = new Chromosome();
        var child2 = new Chromosome();

        int geneCount = parent1.Genes.Count;

        for (int i = 0; i < geneCount; i++)
        {
            if (i % 2 == 0)
            {
                child1.Genes.Add(parent1.Genes[i]);
                child2.Genes.Add(parent2.Genes[i]);
            }
            else
            {
                child1.Genes.Add(parent2.Genes[i]);
                child2.Genes.Add(parent1.Genes[i]);
            }
        }

        return (child1, child2);
    }
}
