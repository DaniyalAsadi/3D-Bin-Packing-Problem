namespace _3D_Bin_Packing_Problem.Core.Models;

public enum LoadType
{
    /// <summary>
    /// معمولی
    /// </summary>
    Normal,
    /// <summary>
    /// خطرناک 
    /// </summary>
    Hazardous, 
    /// <summary>
    /// مایع
    /// </summary>
    Liquid, 
    /// <summary>
    /// یخ زده
    /// </summary>
    Refrigerated,
    /// <summary>
    /// بزرگ
    /// </summary>
    Oversized
}