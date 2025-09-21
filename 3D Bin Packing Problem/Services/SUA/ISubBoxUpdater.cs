using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.SUA;
internal interface ISubBoxUpdater
{
    List<SubBox> UpdateSubBoxes(SubBox original, Product product, (int L, int W, int H) orientation);
}
