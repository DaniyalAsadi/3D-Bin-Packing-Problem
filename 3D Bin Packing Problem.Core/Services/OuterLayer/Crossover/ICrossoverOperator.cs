using _3D_Bin_Packing_Problem.Core.Model;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;

public interface ICrossoverOperator
{
    (Chromosome, Chromosome) Crossover(Chromosome c1, Chromosome c2);
}