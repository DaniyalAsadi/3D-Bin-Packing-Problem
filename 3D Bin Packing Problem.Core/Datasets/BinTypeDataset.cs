using _3D_Bin_Packing_Problem.Core.Models;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Datasets;

public class BinTypeDataset
{
    public static List<BinType> StandardBinTypes() => new List<BinType>
    {
        BinType.Create(
            name: "Size 1",
            new Dimensions(150,100,100),
            maxWeight: 20,
            cost: 63_800m
        ),

        BinType.Create(
            name: "Size 2",
            new Dimensions(200,150,100),
            maxWeight: 30,
            cost: 115_500m
        ),

        BinType.Create(
            name: "Size 3",
            new Dimensions(200, 200, 150),
            maxWeight: 40,
            cost: 172_700m
        ),

        BinType.Create(
            name: "Size 4",
            new Dimensions(300, 200, 200),
            maxWeight: 50,
            cost: 247_500m
        ),

        BinType.Create(
            name: "Size 5",
            new Dimensions(350,250, 200),
            maxWeight: 60,
            cost: 446_600m
        ),

        BinType.Create(
            name: "Size 6",
            new Dimensions(450,250,200),
            maxWeight: 70,
            cost: 559_900m
        ),

        BinType.Create(
            name: "Size 7",
            new Dimensions(400, 300, 250),
            maxWeight: 80,
            cost: 686_400m
        ),

        BinType.Create(
            name: "Size 8",
            new Dimensions(450,400,300),
            maxWeight: 100,
            cost: 1_043_900m
        ),

        BinType.Create(
            name: "Size 9",
            new Dimensions(550, 450, 350),
            maxWeight: 120,
            cost: 1_375_000m
        )
    };
}
