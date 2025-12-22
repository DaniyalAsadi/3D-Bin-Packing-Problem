using _3D_Bin_Packing_Problem.Core.Models;
using System;
using System.Collections.Generic;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.ItemOrderingStrategy;

public class ItemOrderingStrategyFactory
{
    public IItemOrderingStrategy Create(ItemOrderingStrategyType strategyType, IEnumerable<BinType> binTypes)
    {
        return strategyType switch
        {
            ItemOrderingStrategyType.I1 => new ItemOrderingStrategyI1(),
            ItemOrderingStrategyType.I2 => new ItemOrderingStrategyI2(),
            ItemOrderingStrategyType.I3 => new ItemOrderingStrategyI3(binTypes),
            _ => throw new ArgumentOutOfRangeException(nameof(strategyType))
        };
    }
}