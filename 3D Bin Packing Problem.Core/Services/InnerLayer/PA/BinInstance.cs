using _3D_Bin_Packing_Problem.Core.Model;
using System;

namespace _3D_Bin_Packing_Problem.Core.Services.InnerLayer.PA;

public class BinInstance
{
    public BinInstance(BinType binType)
    {
        BinType = binType;
    }

    public BinType BinType { get; set; }
    public Guid ClonedInstance { get; set; } = Guid.NewGuid();

    public static implicit operator BinInstance(BinType binType)
    {
        return new BinInstance(binType);
    }
}