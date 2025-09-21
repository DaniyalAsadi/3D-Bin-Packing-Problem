using _3D_Bin_Packing_Problem.Model;
using _3D_Bin_Packing_Problem.Services.SBPA;

namespace _3D_Bin_Packing_Problem.Services.PA.Implementation;
public class PlacementAlgorithm(ISingleBoxPacker singleBoxPacker) : IPlacementAlgorithm
{
    public List<Box> PackAllProducts(List<Product> allProducts, List<Box> availableBoxTypes)
    {
        var remainingProducts = new List<Product>(allProducts);
        var usedBoxes = new List<Box>();


        while (remainingProducts.Any())
        {
            Box bestBox = null;
            List<Product> copyProducts = new List<Product>(remainingProducts);


            foreach (var boxType in availableBoxTypes)
            {
                var box = boxType.Clone();


                var testProducts = new List<Product>(copyProducts);
                singleBoxPacker.PackProducts(box, testProducts);


                if (box.PackedProducts.Any() && (bestBox == null || box.PackedProducts.Count > bestBox.PackedProducts.Count))
                {
                    bestBox = box;
                }
            }


            if (bestBox != null)
            {
                usedBoxes.Add(bestBox);
                foreach (var p in bestBox.PackedProducts)
                {
                    remainingProducts.Remove(p);
                }
            }
            else
            {
                // هیچ جعبه‌ای پیدا نشد که محصول باقی‌مانده را جا دهد، خاتمه
                break;
            }
        }


        return usedBoxes;
    }
}