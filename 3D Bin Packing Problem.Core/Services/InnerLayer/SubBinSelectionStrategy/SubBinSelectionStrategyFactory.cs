using System;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinSelectionStrategy;

public class SubBinSelectionStrategyFactory
{
    public ISubBinSelectionStrategy Create(SubBinSelectionStrategyType strategyType)
    {
        return strategyType switch
        {
            SubBinSelectionStrategyType.B1 => new SubBinSelectionStrategyB1(),
            SubBinSelectionStrategyType.B2 => new SubBinSelectionStrategyB2(),
            SubBinSelectionStrategyType.B3 => new SubBinSelectionStrategyB3(),
            SubBinSelectionStrategyType.B4 => new SubBinSelectionStrategyB4(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategyType), strategyType, null)
        };
    }
}