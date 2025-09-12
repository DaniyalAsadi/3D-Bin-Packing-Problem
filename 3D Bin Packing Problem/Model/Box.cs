namespace _3D_Bin_Packing_Problem.Model;

public class Box(int height, int width, int length, decimal cost)
{
    public Guid Id { get; internal set; } = Guid.NewGuid();
    public int Length { get; internal set; } = length;
    public int Width { get; internal set; } = width;
    public int Height { get; internal set; } = height;
    public int Volume => Width * Height * Length;
    public decimal Cost { get; internal set; } = cost;

    public Box Clone()
    {
        return new Box(height: Height, width: Width, length: Length, cost: Cost);
    }

}