using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover.Implementation;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;

public class CrossoverFactory()
{
    public IEnumerable<ICrossoverOperator> Create(CrossoverType type)
    {
        var result = new List<ICrossoverOperator>();

        if (type.HasFlag(CrossoverType.OnePoint))
            result.Add(new OnePointCrossover());

        if (type.HasFlag(CrossoverType.TwoPoint))
            result.Add(new TwoPointCrossover());

        if (type.HasFlag(CrossoverType.Uniform))
            result.Add(new UniformCrossover());

        if (type.HasFlag(CrossoverType.MultiPoint))
            result.Add(new MultiPointCrossover());

        return result;
    }
}