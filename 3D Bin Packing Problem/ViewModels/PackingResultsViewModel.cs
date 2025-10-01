namespace _3D_Bin_Packing_Problem.ViewModels;

public class PackingResultsViewModel
{
    public bool IsSuccess { get; set; }

    /// <summary>
    /// آیتم‌هایی که با موفقیت در Bin قرار گرفتند
    /// </summary>
    public List<PackedItemViewModel> PackedItems { get; set; } = [];
}