public class ClassInfo
{
    public int BinLength { get; set; }
    public int BinWidth { get; set; }
    public int BinHeight { get; set; }
    public Tuple<int, int> LengthRange { get; set; } = new(0, 0);
    public Tuple<int, int> WidthRange { get; set; } = new(0, 0);
    public Tuple<int, int> HeightRange { get; set; } = new(0, 0);
}