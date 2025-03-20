namespace _3D_Bin_Packing_Problem.Model;

public class Product
{
    public Guid Id { get; internal set; }
    public int Length { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }
    public int Volume => Width * Height * Length;

    public override string ToString()
    {
        return $"Id: {Id}, Length: {Length}, Width: {Width}, Height: {Height}";
    }

}