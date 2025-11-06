using System;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.Crossover;

[Flags]
public enum CrossoverType
{
    OnePoint = 1,
    TwoPoint = 2,
    Uniform = 4,
    MultiPoint = 8,

    // اگر خواستی حالت انتخاب همه:
    All = OnePoint | TwoPoint | Uniform | MultiPoint
}