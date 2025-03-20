namespace _3D_Bin_Packing_Problem.Model;

public class Chromosome
{
    public double Fitness { get; set; }
    public BoxPlacement Placement { get; set; } 

    public Chromosome(List<Product> products, List<Box> avaiableBoxes)
    {
        avaiableBoxes= avaiableBoxes.Where(x=> x.Volume > products.Sum(x=>x.Volume)).ToList();

        int random = new Random().Next(0, avaiableBoxes.Count);

        var selectedBoxes = avaiableBoxes[random].Clone();

        foreach (Product product in products)
        {

        }
    }
}


public class BoxPlacement
{
    public required Box Box { get; set; }
    public List<Product> PlacedProducts { get; set; } = new List<Product>();

    public bool TryPlaceProduct(Product product)
    {
        PlacedProducts.Add(product);
        //check product can places in box or not
        return true;
    }

}
