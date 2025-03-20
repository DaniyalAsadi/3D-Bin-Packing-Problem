using _3D_Bin_Packing_Problem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Bin_Packing_Problem.Services;

public class CrossOverStrategy
{
    public static (Chromosome, Chromosome) Crossover (Chromosome parent1, Chromosome parent2)
    {
        var count = parent1.Placement.PlacedProducts.Count;
        var random = new Random();

        int crossoverPoint1 = random.Next(0, count / 2);
        int crossoverPoint2 = random.Next(crossoverPoint1, count);

        var firstBox = parent1.Placement.Box.Clone();
        var secondBox = parent2.Placement.Box.Clone();

        // Choromosome 1
        var firstParts1 = parent1.Placement.PlacedProducts.GetRange(0, crossoverPoint1);
        var secondParts1 = parent2.Placement.PlacedProducts.GetRange(crossoverPoint1, crossoverPoint2 - crossoverPoint1);
        var thirdParts1 = parent1.Placement.PlacedProducts.GetRange(crossoverPoint2, count - crossoverPoint2);

        var firstProducts = firstParts1.Concat(secondParts1).Concat(thirdParts1).Select(x => x.Product).ToList();

        var firstChromosome = new Chromosome(firstProducts, new List<Box> { firstBox });
        // Choromosome 2
        var firstParts2 = parent2.Placement.PlacedProducts.GetRange(0, crossoverPoint1);
        var secondParts2 = parent1.Placement.PlacedProducts.GetRange(crossoverPoint1, crossoverPoint2 - crossoverPoint1);
        var thirdParts2 = parent2.Placement.PlacedProducts.GetRange(crossoverPoint2, count - crossoverPoint2);

        var secondProducts = firstParts2.Concat(secondParts2).Concat(thirdParts2).Select(x => x.Product).ToList();

        var secondChromosome = new Chromosome(secondProducts, new List<Box> { secondBox });

        return (firstChromosome, secondChromosome);
    }
}
