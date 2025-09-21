using _3D_Bin_Packing_Problem.Extensions;
using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.PFCA;
using _3D_Bin_Packing_Problem.Services.SUA.Implementation;

namespace _3D_Bin_Packing_Problem.Services.SBPA.Implementation;
public class SingleBoxPacker(IPlacementFeasibilityChecker placementFeasibilityChecker, SubBoxUpdater subBoxUpdater) : ISingleBoxPacker
{
    public void PackProducts(Box box, List<Product> products)
    {
        var subBoxes = new List<SubBox>
        {
            new SubBox
            {
                X = 0,
                Y = 0,
                Z = 0,
                Length = box.Length,
                Width = box.Width,
                Height = box.Height
            }
        };


        foreach (var product in products.ToList())
        {
            bool placed = false;
            foreach (var subBox in subBoxes.ToList())
            {
                var (status, result) = placementFeasibilityChecker.CanPlaceProduct(product, subBox);
                if (status == PlacementStatus.Success)
                {
                    box.PackedProducts.Add(product);
                    products.Remove(product);
                    placed = true;


                    var orientation = product.GetOrientations()[result.OrientationIndex];
                    var newSubBoxes = subBoxUpdater.UpdateSubBoxes(subBox, product, orientation);


                    subBoxes.Remove(subBox);
                    subBoxes.AddRange(newSubBoxes);
                    break;
                }
            }


            if (!placed)
            {
                // محصول جا نمی‌شه، رد می‌کنیم
                continue;
            }
        }
    }
}
