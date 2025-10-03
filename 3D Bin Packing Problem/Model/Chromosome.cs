using _3D_Bin_Packing_Problem.ViewModels;
using System.Collections;

namespace _3D_Bin_Packing_Problem.Model;

public class Chromosome(List<GeneSequence> geneSequences) : IList<GeneSequence>
{
    private double? _fitness;
    private PackingResultsViewModel? _packingResults;
    public List<GeneSequence> GeneSequences { get; set; } = geneSequences;
    public IEnumerator<GeneSequence> GetEnumerator()
    {
        return GeneSequences.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void Add(GeneSequence item)
    {
        GeneSequences.Add(item);
    }
    public void Clear()
    {
        GeneSequences.Clear();
    }
    public bool Contains(GeneSequence item)
    {
        return GeneSequences.Contains(item);
    }
    public void CopyTo(GeneSequence[] array, int arrayIndex)
    {
        GeneSequences.CopyTo(array, arrayIndex);
    }
    public bool Remove(GeneSequence item)
    {
        return GeneSequences.Remove(item);
    }
    public int Count => GeneSequences.Count;
    public bool IsReadOnly => false;
    public int IndexOf(GeneSequence item)
    {
        return GeneSequences.IndexOf(item);
    }
    public void Insert(int index, GeneSequence item)
    {
        GeneSequences.Insert(index, item);
    }
    public void RemoveAt(int index)
    {
        GeneSequences.RemoveAt(index);
    }
    public GeneSequence this[int index]
    {
        get => GeneSequences[index];
        set => GeneSequences[index] = value;
    }

    public double? Fitness => _fitness;
    public PackingResultsViewModel? PackingResults => _packingResults;
    public void SetFitness(FitnessResultViewModel fitnessResultViewModel)
    {
        _fitness = fitnessResultViewModel.Fitness;
        _packingResults = fitnessResultViewModel.PackingResults;
    }
    public Chromosome Clone()
    {
        return new Chromosome(GeneSequences);

    }


    public override string ToString()
    {
        return $"Count : {Count}, Fitness :{Fitness}";
    }
}