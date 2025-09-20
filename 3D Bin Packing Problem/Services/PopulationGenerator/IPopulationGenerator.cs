using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PopulationGenerator;

public interface IPopulationGenerator
{
    List<Chromosome> Generate(List<Product> itemList, int populationSize, int binTypeCount);
}
