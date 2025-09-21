using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PFCA.Implementation;
public class PlacementFeasibilityChecker : IPlacementFeasibilityChecker
{
    public (PlacementStatus status, PlacementResult? result) CanPlaceProduct(Product product, SubBox subBox, double minSupportRatio = 0.75)
    {
        var orientations = product.GetOrientations();


        for (int i = 0; i < orientations.Count; i++)
        {
            var (pL, pW, pH) = orientations[i];


            bool fits = pL <= subBox.Length && pW <= subBox.Width && pH <= subBox.Height;
            if (!fits) continue;


            double supportArea = pL * pW;
            double baseArea = subBox.Length * subBox.Width;
            double supportRatio = supportArea / baseArea;


            if (supportRatio >= minSupportRatio)
            {
                return (PlacementStatus.Success, new PlacementResult
                {
                    Product = product,
                    SubBox = subBox,
                    OrientationIndex = i
                });
            }
        }


        return (PlacementStatus.Failed, null);
    }
}