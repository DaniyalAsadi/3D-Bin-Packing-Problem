public class ClassInfo
{
    public int BinLength { get; set; }
    public int BinWidth { get; set; }
    public int BinHeight { get; set; }
    public Tuple<int, int> LengthRange { get; set; }
    public Tuple<int, int> WidthRange { get; set; }
    public Tuple<int, int> HeightRange { get; set; }
}