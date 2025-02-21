using static _3D_Bin_Packing_Problem.Program;

namespace _3D_Bin_Packing_Problem.CrossOvers;

public class DoublePointCrossOver : ICrossOver
{
    public (Chromosome, Chromosome) CrossOver(Chromosome parent1, Chromosome parent2)
    {
        Random random = new Random();
        int geneCount = parent1.Genes.Count;

        // Ensure both parents have the same number of genes
        if (geneCount != parent2.Genes.Count)
        {
            throw new ArgumentException("Both parents must have the same number of genes.");
        }

        // Select two crossover points
        int point1 = random.Next(0, geneCount);
        int point2 = random.Next(0, geneCount);

        // Ensure point1 is less than point2
        if (point1 > point2)
        {
            int temp = point1;
            point1 = point2;
            point2 = temp;
        }

        // Create offspring chromosomes
        Chromosome offspring1 = new Chromosome();
        Chromosome offspring2 = new Chromosome();

        // Perform crossover between the two points
        for (int i = 0; i < geneCount; i++)
        {
            if (i >= point1 && i <= point2)
            {
                offspring1.Genes.Add(parent2.Genes[i]);
                offspring2.Genes.Add(parent1.Genes[i]);
            }
            else
            {
                offspring1.Genes.Add(parent1.Genes[i]);
                offspring2.Genes.Add(parent2.Genes[i]);
            }
        }

        // Return the two offspring
        return (offspring1, offspring2);
    }
}
