using System;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.SubBinOrderingStrategy;

public class SubBinOrderingStrategyFactory
{
    public ISubBinOrderingStrategy Create(SubBinOrderingStrategyType strategyType)
    {
        return strategyType switch
        {
            SubBinOrderingStrategyType.S1 => new SubBinOrderingStrategyS1(),
            SubBinOrderingStrategyType.S2 => new SubBinOrderingStrategyS2(),
            SubBinOrderingStrategyType.S3 => new SubBinOrderingStrategyS3(),
            SubBinOrderingStrategyType.S4 => new SubBinOrderingStrategyS4(),
            SubBinOrderingStrategyType.S5 => new SubBinOrderingStrategyS5(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategyType))
        };
    }
}