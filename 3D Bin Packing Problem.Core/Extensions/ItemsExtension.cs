using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Extensions
{
    /// <summary>
    /// Provides helper extensions for working with product items.
    /// </summary>
    public static class ItemsExtension
    {
        // گرفتن همه حالت‌های چرخش (۶ حالت ممکن)
        public static IEnumerable<Dimensions> GetOrientations(this Item item)
        {
            return item.Orientations.Select(orientation => orientation switch
            {
                Orientation.Xy => new Dimensions(item.Dimensions.Length, item.Dimensions.Width, item.Dimensions.Height),
                Orientation.Xz => new Dimensions(item.Dimensions.Length, item.Dimensions.Height, item.Dimensions.Width),
                Orientation.Yx => new Dimensions(item.Dimensions.Width, item.Dimensions.Length, item.Dimensions.Height),
                Orientation.Yz => new Dimensions(item.Dimensions.Width, item.Dimensions.Height, item.Dimensions.Length),
                Orientation.Zx => new Dimensions(item.Dimensions.Height, item.Dimensions.Length, item.Dimensions.Width),
                Orientation.Zy => new Dimensions(item.Dimensions.Height, item.Dimensions.Width, item.Dimensions.Length),
                _ => throw new ArgumentOutOfRangeException()
            });
        }
    }
}
