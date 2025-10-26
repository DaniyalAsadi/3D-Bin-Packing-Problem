using _3D_Bin_Packing_Problem.Core.Model;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Mutation;

public interface IMutationOperator
{
    Chromosome Mutate(Chromosome chromosome);
}

