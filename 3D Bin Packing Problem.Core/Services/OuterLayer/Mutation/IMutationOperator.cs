using _3D_Bin_Packing_Problem.Core.Models;
using _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation.Implementation;
using System;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation;

public interface IMutationOperator
{
    Chromosome Mutate(Chromosome chromosome);
}

[Flags]
public enum MutationType
{
    OnePoint,
    TwoPoint,

    All = OnePoint | TwoPoint
}
public class MutationFactory()
{
    public IEnumerable<IMutationOperator> Create(MutationType type)
    {
        var result = new List<IMutationOperator>();

        if (type.HasFlag(MutationType.OnePoint))
            result.Add(new OnePointMutation());

        if (type.HasFlag(MutationType.TwoPoint))
            result.Add(new TwoPointMutation());

        return result;
    }
}