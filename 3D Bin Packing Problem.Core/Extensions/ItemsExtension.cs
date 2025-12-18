using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Core.Extensions
{
    /// <summary>
    /// Provides helper extensions for working with product items.
    /// </summary>
    public static class ItemsExtension
    {
        // گرفتن همه حالت‌های چرخش (۶ حالت ممکن)
        public static IEnumerable<Vector3> GetOrientations(this Item item)
        {
            return item.Orientations.Select(orientation => orientation switch
            {
                Orientation.Xy => new Vector3(item.Dimensions.Length, item.Dimensions.Width, item.Dimensions.Height),
                Orientation.Xz => new Vector3(item.Dimensions.Length, item.Dimensions.Height, item.Dimensions.Width),
                Orientation.Yx => new Vector3(item.Dimensions.Width, item.Dimensions.Length, item.Dimensions.Height),
                Orientation.Yz => new Vector3(item.Dimensions.Width, item.Dimensions.Height, item.Dimensions.Length),
                Orientation.Zx => new Vector3(item.Dimensions.Height, item.Dimensions.Length, item.Dimensions.Width),
                Orientation.Zy => new Vector3(item.Dimensions.Height, item.Dimensions.Width, item.Dimensions.Length),
                _ => throw new ArgumentOutOfRangeException()
            });
        }
    }
}
