namespace _3D_Bin_Packing_Problem.Models;
public class Box
{
    public int Id { get; set; }
    /// <summary>
    /// Length of the box : x-axis
    /// </summary>
    public int Length { get; set; }
    /// <summary>
    /// Width of the box : y-axis
    /// </summary>
    public int Height { get; set; }
    /// <summary>
    /// Hight of the box : z-axis
    /// </summary>
    public int Width { get; set; }

    public int Price { get; set; }
    public int Volume => Length * Width * Height;
    public override string ToString() => $"Box {Id} ({Length}x{Height}x{Width}) : Volume({Volume})";
}
