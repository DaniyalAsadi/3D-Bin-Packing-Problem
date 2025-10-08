namespace _3D_Bin_Packing_Problem.Model;

/// <summary>
/// Represents an item with dimensions and volume information for packing.
/// </summary>
public class Item(int length, int width, int height)
{
    public Guid Id { get; internal set; } = Guid.NewGuid();
    public int Length { get; internal set; } = length;
    public int Width { get; internal set; } = width;
    public int Height { get; internal set; } = height;
    public int Volume => Width * Height * Length;

    public override string ToString()
    {
        return $"Length: {Length}, Width: {Width}, Height: {Height}";
    }
    public int GetMinimumDimension()
    {
        return Math.Min(Length, Math.Min(Width, Height));
    }
}
