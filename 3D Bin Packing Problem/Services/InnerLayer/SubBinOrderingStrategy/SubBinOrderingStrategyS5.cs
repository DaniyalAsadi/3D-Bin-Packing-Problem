using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.InnerLayer.SubBinOrderingStrategy;

public class SubBinOrderingStrategyS5 : ISubBinOrderingStrategy
{
    private double ComputeCornerDistance(SubBin sb)
    {
        return Math.Sqrt(sb.X * sb.X + sb.Y * sb.Y + sb.Z * sb.Z);
    }

    public IEnumerable<SubBin> Apply(IEnumerable<SubBin> subBins, Item item)
    {
        return subBins
            .OrderBy(ComputeCornerDistance)
            .ThenByDescending(sb => sb.Volume)
            .ThenBy(sb => sb.X)
            .ThenBy(sb => sb.Y)
            .ThenBy(sb => sb.Z)
            .ThenBy(_ => item.Id);
    }
}