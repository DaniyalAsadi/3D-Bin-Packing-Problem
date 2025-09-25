namespace _3D_Bin_Packing_Problem.ViewModels;
public class PackingResultViewModel
{
    /// <summary>
    /// آیتم‌هایی که با موفقیت در Bin قرار گرفتند
    /// </summary>
    public List<PackedItemViewModel> PackedItems { get; set; } = new();

    /// <summary>
    /// لیست Sub-binهای باقی‌مانده بعد از آخرین مرحله SUA
    /// </summary>
    public List<SubBinViewModel> RemainingSubBins { get; set; } = new();

    /// <summary>
    /// آیتم‌هایی که در Bin جا نشدند
    /// </summary>
    public List<ItemViewModel> LeftItems { get; set; } = new();
}

/// <summary>
/// نمایش یک آیتم که در Bin بسته‌بندی شده
/// </summary>
public class PackedItemViewModel
{
    public Guid ItemId { get; set; }

    // موقعیت قرارگیری (گوشه پایین-چپ-پشت)
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    // اورینتیشن آیتم در لحظه قرارگیری
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    // نسبت تکیه‌گاه
    public double SupportRatio { get; set; }
}

/// <summary>
/// نمایش Sub-bin باقی‌مانده
/// </summary>
public class SubBinViewModel
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

/// <summary>
/// نمایش یک آیتم جا نشده
/// </summary>
public class ItemViewModel
{
    public Guid Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

