using static _3D_Bin_Packing_Problem.Program;

namespace _3D_Bin_Packing_Problem.CrossOvers;

public interface ICrossOver
{
    (Chromosome, Chromosome) CrossOver(Chromosome parent1, Chromosome parent2);
}
public class CrossoverFactory
{
    public static ICrossOver GetCrossover(CrossoverType crossoverType)
    {
        return crossoverType switch
        {
            CrossoverType.Single => new SingleCrossOver(),
            CrossoverType.DoublePoint => new DoublePointCrossOver(),
            CrossoverType.Uniform => new UniformCrossOver(),
            CrossoverType.Arithmetic => new ArithmeticCrossOver(),
            _ => throw new NotImplementedException()
        };
    }
}
public enum CrossoverType
{
    Single,
    DoublePoint,
    Uniform,
    Arithmetic
}