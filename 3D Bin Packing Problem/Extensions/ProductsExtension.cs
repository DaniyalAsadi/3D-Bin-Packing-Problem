using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Extensions
{
    public static class ProductsExtension
    {
        public static Vector3 ToVector3D(this Product product)
        {
            return new Vector3 { X = product.Length, Y = product.Height, Z = product.Width };
        }

    }
}
