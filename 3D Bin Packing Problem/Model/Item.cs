namespace _3D_Bin_Packing_Problem.Model;

public class Item
{
    public Guid Id { get; internal set; }
    public int Length { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }
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