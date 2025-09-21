using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PFCA.Implementation;

public class PlacementFeasibilityChecker : IPlacementFeasibilityChecker
{
    public bool CanPlaceProduct(Product product, SubBox subBox, out PlacementResult? result, double minSupportRatio = 0.75)
    {
        var orientations = product.GetOrientations();
        for (var i = 0; i < orientations.Count; i++)
        {
            var (pL, pW, pH) = orientations[i];
            var fits = pL <= subBox.Length && pW <= subBox.Width && pH <= subBox.Height;
            if (!fits) continue;
            double supportArea = pL * pW;
            double baseArea = subBox.Length * subBox.Width;
            var supportRatio = supportArea / baseArea;
            if (!(supportRatio >= minSupportRatio)) continue;
            result = new PlacementResult
            {
                Product = product,
                SubBox = subBox,
                OrientationIndex = i

            };
            return true;
        }

        result = null;
        return false;
    }
}