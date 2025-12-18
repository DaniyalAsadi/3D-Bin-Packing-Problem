using System;

namespace _3D_Bin_Packing_Problem.Core.Model;

public class BinInstance(BinType binType)
{
    public BinType BinType { get; set; } = binType;
    public Guid ClonedInstance { get; set; } = Guid.NewGuid();

}