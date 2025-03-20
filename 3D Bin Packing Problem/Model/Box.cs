namespace _3D_Bin_Packing_Problem.Model;

public class Box
{
    public Guid Id { get; internal set; }
    public int Length { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }    
    public int Volume => Width * Height * Length;

    public Box Clone()
    {
        return new Box { Id = Guid.NewGuid(), Height = Height, Width = Width, Length = Length };  
    }

}