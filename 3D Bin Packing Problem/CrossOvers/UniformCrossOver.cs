using static _3D_Bin_Packing_Problem.Program;

namespace _3D_Bin_Packing_Problem.CrossOvers;

public class UniformCrossOver : ICrossOver
{
    private readonly Random _random;
    public UniformCrossOver()
    {
        _random = new Random();
    }
    public (Chromosome, Chromosome) CrossOver(Chromosome parent1, Chromosome parent2)
    {
        var offspring1 = new Chromosome();
        var offspring2 = new Chromosome();

        for (int i = 0; i < parent1.Genes.Count; i++)
        {
            if (_random.NextDouble() < 0.5)
            {
                offspring1.Genes.Add(parent1.Genes[i]);
                offspring2.Genes.Add(parent2.Genes[i]);
            }
            else
            {
                offspring1.Genes.Add(parent2.Genes[i]);
                offspring2.Genes.Add(parent1.Genes[i]);
            }
        }

        return (offspring1, offspring2);
    }
}