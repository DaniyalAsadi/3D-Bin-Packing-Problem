using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.SASSManager;
public interface ISassManager
{
    void InitStructure(Box box);
    void UpdateStructure(Box box, Product product);
}
