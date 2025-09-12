using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.Mutation.Implementation;
internal class TwoPointMutation : IMutationOperator
{
    private static readonly Random Random = new Random(3);
    public Chromosome Mutate(Chromosome c)
    {
        if (c.Count < 2) return new Chromosome(c); // nothing to mutate

        var mutated = new Chromosome(c); // clone
        int p1 = Random.Next(0, mutated.Count);
        int p2 = Random.Next(0, mutated.Count);

        // make sure p1 != p2
        while (p1 == p2)
            p2 = Random.Next(0, mutated.Count);

        // swap genes
        (mutated[p1], mutated[p2]) = (mutated[p2], mutated[p1]);

        return mutated;
    }
}
