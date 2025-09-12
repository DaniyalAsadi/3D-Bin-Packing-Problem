using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Mutation;

public interface IMutationOperator
{
    Chromosome Mutate(Chromosome chromosome);
}

