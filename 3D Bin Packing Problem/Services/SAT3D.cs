namespace _3D_Bin_Packing_Problem.Services
{
    using System.Collections.Generic;
    using System.Numerics;

    public class SAT3D
    {
        // بررسی برخورد دو جسم سه‌بعدی
        public static bool IsColliding(Vector3[] polyA, Vector3[] polyB)
        {
            List<Vector3> axes = new List<Vector3>();

            // محورها را از نرمال سطوح دو جسم استخراج می‌کنیم
            axes.AddRange(GetFaceNormals(polyA));
            axes.AddRange(GetFaceNormals(polyB));

            // همچنین باید حاصل‌ضرب برداری بین اضلاع دو جسم را به عنوان محور بررسی کنیم
            axes.AddRange(GetEdgeCrossProducts(polyA, polyB));

            // بررسی برخورد بر روی هر محور
            foreach (Vector3 axis in axes)
            {
                if (axis.LengthSquared() < 1e-6) continue; // حذف محورها با مقدار صفر

                if (!OverlapOnAxis(polyA, polyB, axis))
                    return false; // برخوردی وجود ندارد
            }

            return true; // همه محورها همپوشانی دارند، پس برخورد وجود دارد
        }

        // دریافت نرمال سطوح برای یک جسم
        private static List<Vector3> GetFaceNormals(Vector3[] poly)
        {
            List<Vector3> axes = new List<Vector3>();

            for (int i = 0; i < poly.Length; i += 3)
            {
                // یک سطح سه‌ضلعی را انتخاب می‌کنیم
                Vector3 p1 = poly[i];
                Vector3 p2 = poly[(i + 1) % poly.Length];
                Vector3 p3 = poly[(i + 2) % poly.Length];

                // محاسبه نرمال سطح
                Vector3 edge1 = p2 - p1;
                Vector3 edge2 = p3 - p1;
                Vector3 normal = Vector3.Cross(edge1, edge2);
                normal = Vector3.Normalize(normal);

                axes.Add(normal);
            }

            return axes;
        }

        // دریافت محورها از حاصل‌ضرب برداری اضلاع دو جسم
        private static List<Vector3> GetEdgeCrossProducts(Vector3[] polyA, Vector3[] polyB)
        {
            List<Vector3> axes = new List<Vector3>();

            for (int i = 0; i < polyA.Length; i++)
            {
                Vector3 edgeA = polyA[(i + 1) % polyA.Length] - polyA[i];

                for (int j = 0; j < polyB.Length; j++)
                {
                    Vector3 edgeB = polyB[(j + 1) % polyB.Length] - polyB[j];

                    Vector3 axis = Vector3.Cross(edgeA, edgeB);
                    if (axis.LengthSquared() > 1e-6) // حذف محورها با مقدار صفر
                        axes.Add(Vector3.Normalize(axis));
                }
            }

            return axes;
        }

        // بررسی همپوشانی دو چندوجهی روی محور خاص
        private static bool OverlapOnAxis(Vector3[] polyA, Vector3[] polyB, Vector3 axis)
        {
            (float minA, float maxA) = ProjectPolygon(polyA, axis);
            (float minB, float maxB) = ProjectPolygon(polyB, axis);

            return !(maxA < minB || maxB < minA); // اگر هیچ همپوشانی‌ای وجود ندارد، برخورد نیست
        }

        // فرافکنی یک چندوجهی روی محور
        private static (float, float) ProjectPolygon(Vector3[] poly, Vector3 axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (Vector3 point in poly)
            {
                float projection = Vector3.Dot(point, axis);
                if (projection < min) min = projection;
                if (projection > max) max = projection;
            }

            return (min, max);
        }

        // تست عملکرد
        //public static void Main()
        //{
        //    var x = 0.5f;
        //    var y = 0.5f;
        //    var z = 0.5f;
        //    Vector3 middle = new Vector3(x, y, z);


        //    // دو مکعب که ممکن است برخورد داشته باشند
        //    Vector3[] cubeA = {
        //    new Vector3(0, 0, 0),
        //    new Vector3(1, 0, 0),
        //    new Vector3(1, 1, 0),
        //    new Vector3(0, 1, 0),
        //    new Vector3(0, 0, 1),
        //    new Vector3(1, 0, 1),
        //    new Vector3(1, 1, 1),
        //    new Vector3(0, 1, 1)
        //    };

        //    Vector3[] cubeB = {
        //    new Vector3(2f, 2f,2f),
        //    new Vector3(3f, 2f, 2f),
        //    new Vector3(3f, 3f, 2f),
        //    new Vector3(2f, 3f, 2f),
        //    new Vector3(2f, 2f, 3f),
        //    new Vector3(3f, 2f, 3f),
        //    new Vector3(3f, 3f, 3f),
        //    new Vector3(2f, 3f, 3f)
        //};

        //    bool collision = IsColliding(cubeA, cubeB);
        //    //Console.WriteLine(collision ? "💥 برخورد دارد!" : "✅ برخورد ندارد!");
        //    Console.WriteLine(collision);
        //}
    }
}
