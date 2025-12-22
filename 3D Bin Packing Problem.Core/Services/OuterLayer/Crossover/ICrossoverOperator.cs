using _3D_Bin_Packing_Problem.Core.Models;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;

public interface ICrossoverOperator
{
    IEnumerable<Chromosome> Crossover(Chromosome c1, Chromosome c2);
}