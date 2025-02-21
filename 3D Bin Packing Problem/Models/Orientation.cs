namespace _3D_Bin_Packing_Problem.Models;

public enum Orientation
{
    /// <summary>
    /// Length, Height, Width
    /// </summary>
    LHW,
    /// <summary>
    /// Length, Width, Height
    /// </summary>
    LWH,
    /// <summary>
    /// Height, Length, Width
    /// </summary>
    HLW,
    /// <summary>
    /// Height, Width, Length
    /// </summary>
    HWL,
    /// <summary>
    /// Width, Length, Height
    /// </summary>
    WLH,
    /// <summary>
    /// Width, Height, Length
    /// </summary>
    WHL,
}
public static class OrientationHelper
{
    public static (int Length, int Height, int Width) GetDimensions(Product product, Orientation orientation)
    {
        return orientation switch
        {
            Orientation.LHW => (product.Length, product.Height, product.Width),
            Orientation.LWH => (product.Length, product.Width, product.Height),
            Orientation.HLW => (product.Height, product.Length, product.Width),
            Orientation.HWL => (product.Height, product.Width, product.Length),
            Orientation.WLH => (product.Width, product.Length, product.Height),
            Orientation.WHL => (product.Width, product.Height, product.Length),
            _ => throw new ArgumentException("Invalid orientation"),
        };
    }
}