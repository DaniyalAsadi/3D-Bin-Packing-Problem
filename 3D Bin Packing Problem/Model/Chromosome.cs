namespace _3D_Bin_Packing_Problem.Model;

public class Chromosome : IList<Gene>, IEquatable<Chromosome>
{
    private readonly List<Gene> _genes;

    public Chromosome(IEnumerable<Gene> genes)
    {
        _genes = new List<Gene>(genes);
    }

    internal Chromosome(Chromosome chromosome) : this(chromosome._genes)
    {
    }

    public List<Gene> Genes => _genes;

    // محاسبه مجموع هزینه انواع Box (صرفاً طراحی)
    public decimal TotalCost(double unitCost = 0.001)
    {
        return _genes.Sum(g => g.Cost(unitCost));
    }

    public decimal Fitness => TotalCost();

    // دیکد به لیست BoxType (برای استفاده در Packing Algorithm)
    public List<Box> DecodeToBoxes(double unitCost = 0.001)
    {
        return _genes.Select(t => t.ToBox(unitCost)).ToList();
    }

    public override string ToString()
    {
        return $"Chromosome with {Count} genes:\n" +
               string.Join("\n", _genes.Select(g => "  " + g));
    }

    public bool Equals(Chromosome? other)
    {
        return other != null && Fitness.Equals(other.Fitness);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            Chromosome otherChromosome => Equals(otherChromosome),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return Fitness.GetHashCode();
    }

    // IList<Gene> implementation (delegates to _genes)
    public Gene this[int index] { get => _genes[index]; set => _genes[index] = value; }
    public int Count => _genes.Count;
    public bool IsReadOnly => false;

    public void Add(Gene item) => _genes.Add(item);
    public void Clear() => _genes.Clear();
    public bool Contains(Gene item) => _genes.Contains(item);
    public void CopyTo(Gene[] array, int arrayIndex) => _genes.CopyTo(array, arrayIndex);
    public IEnumerator<Gene> GetEnumerator() => _genes.GetEnumerator();
    public int IndexOf(Gene item) => _genes.IndexOf(item);
    public void Insert(int index, Gene item) => _genes.Insert(index, item);
    public bool Remove(Gene item) => _genes.Remove(item);
    public void RemoveAt(int index) => _genes.RemoveAt(index);
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _genes.GetEnumerator();
}
