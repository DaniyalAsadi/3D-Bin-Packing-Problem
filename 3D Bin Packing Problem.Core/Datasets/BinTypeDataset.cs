using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Datasets;

public class BinTypeDataset
{
    public static List<BinType> StandardBinTypes() => new List<BinType>
    {
        BinType.Create(
            name: "Size 1",
            length: 150,
            width: 100,
            height: 100,
            maxWeight: 20,
            cost: 63_800m
        ),

        BinType.Create(
            name: "Size 2",
            length: 200,
            width: 150,
            height: 100,
            maxWeight: 30,
            cost: 115_500m
        ),

        BinType.Create(
            name: "Size 3",
            length: 200,
            width: 200,
            height: 150,
            maxWeight: 40,
            cost: 172_700m
        ),

        BinType.Create(
            name: "Size 4",
            length: 300,
            width: 200,
            height: 200,
            maxWeight: 50,
            cost: 247_500m
        ),

        BinType.Create(
            name: "Size 5",
            length: 350,
            width: 250,
            height: 200,
            maxWeight: 60,
            cost: 446_600m
        ),

        BinType.Create(
            name: "Size 6",
            length: 450,
            width: 250,
            height: 200,
            maxWeight: 70,
            cost: 559_900m
        ),

        BinType.Create(
            name: "Size 7",
            length: 400,
            width: 300,
            height: 250,
            maxWeight: 80,
            cost: 686_400m
        ),

        BinType.Create(
            name: "Size 8",
            length: 450,
            width: 400,
            height: 300,
            maxWeight: 100,
            cost: 1_043_900m
        ),

        BinType.Create(
            name: "Size 9",
            length: 550,
            width: 450,
            height: 350,
            maxWeight: 120,
            cost: 1_375_000m
        )
    };
}
