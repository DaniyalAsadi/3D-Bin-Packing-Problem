using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.ViewModels;

/// <summary>
/// Captures the best sub-bin choice together with the items considered during selection.
/// </summary>
public class SubBinSelectionViewModel
{
    public SubBin BestSubBin { get; set; }

    public List<PackedItemViewModel> Items { get; set; }


    public override string ToString()
    {
        return $"SubBin={BestSubBin}";
    }
}
