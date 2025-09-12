namespace _3D_Bin_Packing_Problem.Model;

public class Gene(Box box)
{
    public Box Box { get; } = box;
    // محاسبه هزینه: مشابه Box (بر اساس سطح خارجی)
    public decimal Cost(double unitCost = 0.001)
    {
        int surface = 2 * (Box.Length * Box.Width + Box.Length * Box.Height + Box.Width * Box.Height);
        return (decimal)(surface * unitCost);
    }

    // دیکد به یک BoxType (ایجاد یک نمونه Box بدون Product)
    public Box ToBox(double unitCost = 0.001)
    {
        return new Box(Box.Length, Box.Width, Box.Height, Cost(unitCost));
    }
}
