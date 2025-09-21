namespace _3D_Bin_Packing_Problem.Model;

public class SubBox
{
    public int X { get; set; } // موقعیت مکانی در جعبه (گوشه پایین-چپ-پشت)
    public int Y { get; set; }
    public int Z { get; set; }


    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }


    public int Back { get; set; }
    public int Left { get; set; }
    public int Front { get; set; }
    public int Right { get; set; }
}