using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Extensions
{
    public static class ProductsExtension
    {
        public static Vector3[] ToVector3(this Product product, Vector3 middle)
        {
            Vector3[] poly = new Vector3[8];
            poly[0] = new Vector3(middle.X - (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[1] = new Vector3(middle.X + (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[2] = new Vector3(middle.X + (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[3] = new Vector3(middle.X - (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[4] = new Vector3(middle.X - (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            poly[5] = new Vector3(middle.X + (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            poly[6] = new Vector3(middle.X + (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            poly[7] = new Vector3(middle.X - (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            return poly;
        }
    }
}
