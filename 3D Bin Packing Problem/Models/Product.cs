namespace _3D_Bin_Packing_Problem.Models;

public class Product : IEquatable<Product>
{
    public int Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Volume => Length * Width * Height;

    public override string ToString() => $"Product {Id} ({Volume})";


    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals(obj as Product);

    }

    public override int GetHashCode() => base.GetHashCode();
    public bool Equals(Product? other)
    {
        if (other is null) return false;
        return
            Length == other.Length &&
            Width == other.Width &&
            Height == other.Height;

    }
}