using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Extensions
{
    public static class ProductsExtension
    {
        public static Vector3[] ToVector3(this Product product, Vector3 middle)
        {
            // نقاط مکعب
            Vector3[] poly = new Vector3[8];
            poly[0] = new Vector3(middle.X - (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[1] = new Vector3(middle.X + (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[2] = new Vector3(middle.X + (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[3] = new Vector3(middle.X - (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z - (float)product.Height / 2);
            poly[4] = new Vector3(middle.X - (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            poly[5] = new Vector3(middle.X + (float)product.Length / 2, middle.Y - (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            poly[6] = new Vector3(middle.X + (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z + (float)product.Height / 2);
            poly[7] = new Vector3(middle.X - (float)product.Length / 2, middle.Y + (float)product.Width / 2, middle.Z + (float)product.Height / 2);

            // تولید مثلث‌ها برای هر یک از 6 وجه مکعب
            List<Vector3> vertices =
            [
                // پایین (face 1)
                poly[0],
                poly[1],
                poly[3],
                poly[1],
                poly[2],
                poly[3],
                // بالا (face 2)
                poly[4],
                poly[5],
                poly[7],
                poly[5],
                poly[6],
                poly[7],
                // جلو (face 3)
                poly[0],
                poly[1],
                poly[4],
                poly[1],
                poly[5],
                poly[4],
                // عقب (face 4)
                poly[3],
                poly[2],
                poly[7],
                poly[2],
                poly[6],
                poly[7],
                // چپ (face 5)
                poly[0],
                poly[3],
                poly[4],
                poly[3],
                poly[7],
                poly[4],
                // راست (face 6)
                poly[1],
                poly[2],
                poly[5],
                poly[2],
                poly[6],
                poly[5],
            ];

            return vertices.ToArray();
        }


        // گرفتن همه حالت‌های چرخش (۶ حالت ممکن)
        public static List<(int L, int W, int H)> GetOrientations(this Product product)
        {
            return new List<(int, int, int)>
            {
                (product.Length, product.Width, product.Height),
                (product.Length, product.Height, product.Width),
                (product.Width, product.Length, product.Height),
                (product.Width, product.Height, product.Length),
                (product.Height, product.Length, product.Width),
                (product.Height, product.Width, product.Length)
            };
        }
    }
}
