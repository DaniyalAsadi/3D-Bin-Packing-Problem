using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.PopulationGenerator;

public interface IPopulationGenerator
{
    List<Chromosome> Generate(List<Item> itemList, int populationSize, int binTypeCount);
}
