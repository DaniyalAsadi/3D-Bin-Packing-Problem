using _3D_Bin_Packing_Problem.Core.Model;
using System.Collections.Generic;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Core.Extensions
{
    /// <summary>
    /// Provides helper extensions for working with product items.
    /// </summary>
    public static class ProductsExtension
    {
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
