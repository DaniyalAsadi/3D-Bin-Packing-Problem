using _3D_Bin_Packing_Problem.Model;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Extensions
{
    public static class ProductsExtension
    {
        public static Vector3[] ToVector3(this Item item, Vector3 middle)
        {
            // نقاط مکعب
            Vector3[] poly = new Vector3[8];
            poly[0] = new Vector3(middle.X - (float)item.Length / 2, middle.Y - (float)item.Width / 2, middle.Z - (float)item.Height / 2);
            poly[1] = new Vector3(middle.X + (float)item.Length / 2, middle.Y - (float)item.Width / 2, middle.Z - (float)item.Height / 2);
            poly[2] = new Vector3(middle.X + (float)item.Length / 2, middle.Y + (float)item.Width / 2, middle.Z - (float)item.Height / 2);
            poly[3] = new Vector3(middle.X - (float)item.Length / 2, middle.Y + (float)item.Width / 2, middle.Z - (float)item.Height / 2);
            poly[4] = new Vector3(middle.X - (float)item.Length / 2, middle.Y - (float)item.Width / 2, middle.Z + (float)item.Height / 2);
            poly[5] = new Vector3(middle.X + (float)item.Length / 2, middle.Y - (float)item.Width / 2, middle.Z + (float)item.Height / 2);
            poly[6] = new Vector3(middle.X + (float)item.Length / 2, middle.Y + (float)item.Width / 2, middle.Z + (float)item.Height / 2);
            poly[7] = new Vector3(middle.X - (float)item.Length / 2, middle.Y + (float)item.Width / 2, middle.Z + (float)item.Height / 2);

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
        public static IEnumerable<Vector3> GetOrientations(this Item item)
        {
            yield return new Vector3(item.Length, item.Width, item.Height);
            yield return new Vector3(item.Length, item.Height, item.Width);
            yield return new Vector3(item.Width, item.Length, item.Height);
            yield return new Vector3(item.Width, item.Height, item.Length);
            yield return new Vector3(item.Height, item.Length, item.Width);
            yield return new Vector3(item.Height, item.Width, item.Length);

        }
    }
}
