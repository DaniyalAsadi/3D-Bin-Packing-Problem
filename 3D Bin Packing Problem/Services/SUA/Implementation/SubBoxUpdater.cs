using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.SUA.Implementation;
public class SubBoxUpdater : ISubBoxUpdater
{
    public List<SubBox> UpdateSubBoxes(SubBox original, Product product, (int L, int W, int H) orientation)
    {
        var newSubBoxes = new List<SubBox>();


        // زیرفضای بالا (Top)
        newSubBoxes.Add(new SubBox
        {
            X = original.X,
            Y = original.Y,
            Z = original.Z + orientation.H,
            Length = original.Length,
            Width = original.Width,
            Height = original.Height - orientation.H,
            Back = original.Back,
            Left = original.Left,
            Front = original.Front,
            Right = original.Right
        });


        // زیرفضای جلو (Front)
        newSubBoxes.Add(new SubBox
        {
            X = original.X,
            Y = original.Y + orientation.W,
            Z = original.Z,
            Length = original.Length,
            Width = original.Width - orientation.W,
            Height = orientation.H,
            Back = original.Back,
            Left = original.Left,
            Front = original.Front,
            Right = original.Right
        });


        // زیرفضای راست (Right)
        newSubBoxes.Add(new SubBox
        {
            X = original.X + orientation.L,
            Y = original.Y,
            Z = original.Z,
            Length = original.Length - orientation.L,
            Width = orientation.W,
            Height = orientation.H,
            Back = original.Back,
            Left = original.Left,
            Front = original.Front,
            Right = original.Right
        });


        // فیلتر کردن subboxهایی که اندازه منفی دارند
        return newSubBoxes.Where(sb => sb.Length > 0 && sb.Width > 0 && sb.Height > 0).ToList();
    }
}