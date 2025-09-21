using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PFCA;
public interface IPlacementFeasibilityChecker
{
    (PlacementStatus status, PlacementResult? result) CanPlaceProduct(Product product, SubBox subBox, double minSupportRatio = 0.75);
}
