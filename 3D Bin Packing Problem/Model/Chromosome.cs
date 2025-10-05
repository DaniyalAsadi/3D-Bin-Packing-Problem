using _3D_Bin_Packing_Problem.ViewModels;
using System.Collections;

namespace _3D_Bin_Packing_Problem.Model;
public class Chromosome : IList<GeneSequence>
{
    public Guid Id { get; } = Guid.NewGuid();
    private double _fitness;
    private PackingResultsViewModel? _packingResults;

    public List<GeneSequence> GeneSequences { get; private set; }

    public Chromosome(List<GeneSequence> geneSequences)
    {
        // clone to avoid shared reference when creating population
        GeneSequences = geneSequences.Select(gs => gs.Clone()).ToList();
    }

    public IEnumerator<GeneSequence> GetEnumerator() => GeneSequences.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(GeneSequence item) => GeneSequences.Add(item);
    public void Clear() => GeneSequences.Clear();
    public bool Contains(GeneSequence item) => GeneSequences.Contains(item);
    public void CopyTo(GeneSequence[] array, int arrayIndex) => GeneSequences.CopyTo(array, arrayIndex);
    public bool Remove(GeneSequence item) => GeneSequences.Remove(item);
    public int Count => GeneSequences.Count;
    public bool IsReadOnly => false;
    public int IndexOf(GeneSequence item) => GeneSequences.IndexOf(item);
    public void Insert(int index, GeneSequence item) => GeneSequences.Insert(index, item);
    public void RemoveAt(int index) => GeneSequences.RemoveAt(index);
    public GeneSequence this[int index]
    {
        get => GeneSequences[index];
        set => GeneSequences[index] = value;
    }

    public double Fitness => _fitness;
    public PackingResultsViewModel? PackingResults => _packingResults;

    public void SetFitness(FitnessResultViewModel fitnessResultViewModel)
    {
        _fitness = fitnessResultViewModel.Fitness;
        _packingResults = fitnessResultViewModel.PackingResults;
    }

    /// <summary>
    /// Deep clone — completely isolates BinType, GeneSequence, and Genes
    /// </summary>
    public Chromosome Clone()
    {
        var clonedSequences = GeneSequences.Select(gs => gs.Clone()).ToList();
        var clone = new Chromosome(clonedSequences);
        clone._fitness = _fitness;
        clone._packingResults = null;
        return clone;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Chromosome chromosome => Id.Equals(chromosome.Id),
            null => false,
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString() => $"Count: {Count}, Fitness: {_fitness}";
}
