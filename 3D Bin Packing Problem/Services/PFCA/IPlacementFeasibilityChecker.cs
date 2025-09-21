using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PFCA;
public interface IPlacementFeasibilityChecker
{
    bool CanPlaceProduct(Product product, SubBox subBox, out PlacementResult? result, double minSupportRatio = 0.75);
}
