using _3D_Bin_Packing_Problem.Models;
using System.Text;

namespace _3D_Bin_Packing_Problem;

public class Chromosome
{
    public List<Gene> Genes { get; set; } = new List<Gene>();
    public List<BoxPlacement> BoxPlacements { get; set; } = new List<BoxPlacement>();
    public double Fitness { get; set; }
    public override string ToString()
    {
        StringBuilder builder = new();
        foreach (var gene in Genes)
        {
            builder.AppendLine(gene.ToString());
        }
        return builder.ToString();
    }
}