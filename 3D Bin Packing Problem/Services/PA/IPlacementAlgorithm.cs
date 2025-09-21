using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PA;

public interface IPlacementAlgorithm
{
    List<Box> PackAllProducts(List<Product> allProducts, List<Box> availableBoxTypes);
}
