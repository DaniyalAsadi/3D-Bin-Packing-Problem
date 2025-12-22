using _3D_Bin_Packing_Problem.Core.Models;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.PopulationGenerator;

public interface IPopulationGenerator
{
    void SetAvailableBins(List<BinType> predefinedBinTypes);
    List<Chromosome> Generate(List<Item> itemList, int populationSize, int binTypeCount);
}
