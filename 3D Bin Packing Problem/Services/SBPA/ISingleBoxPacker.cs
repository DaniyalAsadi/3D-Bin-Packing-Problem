using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.SBPA;

public interface ISingleBoxPacker
{
    void PackProducts(Box box, List<Product> products);
}
