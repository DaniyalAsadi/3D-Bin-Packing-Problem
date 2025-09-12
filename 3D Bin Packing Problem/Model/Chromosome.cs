using _3D_Bin_Packing_Problem.ViewModels;
using System.Collections;

namespace _3D_Bin_Packing_Problem.Model;

public class Chromosome(List<GeneSequence> geneSequences) : IList<GeneSequence>
{
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
    public int Count => GeneSequences.Count * 3;
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

    public IEnumerable<IndexedGene> IndexedGenes
    {
        get
        {
            for (var seqIndex = 0; seqIndex < Count; seqIndex++)
            {
                var seq = GeneSequences[seqIndex];
                for (var geneIndex = 0; geneIndex < seq.Count; geneIndex++)
                {
                    yield return new IndexedGene(seq[geneIndex], seqIndex, geneIndex);
                }
            }
        }
    }

}

public class GeneSequence(Box box) : IEnumerable<Gene>, IEquatable<GeneSequence>
{
    public Box Box { get; private set; } = box;
    public Gene Length { get; private set; } = box.Length;
    public Gene Width { get; private set; } = box.Width;
    public Gene Height { get; private set; } = box.Height;

    private IEnumerable<Gene> Items
    {
        get
        {
            yield return Length;
            yield return Width;
            yield return Height;
        }
    }

    // --- IList implementation ---
    public Gene this[int index]
    {
        get => index switch
        {
            0 => Length,
            1 => Width,
            2 => Height,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
        set
        {
            switch (index)
            {
                case 0: Length = value; break;
                case 1: Width = value; break;
                case 2: Height = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }

    public int Count => 3;
    public bool IsReadOnly => false;

    public IEnumerator<Gene> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int IndexOf(Gene item)
    {
        if (Equals(item, Length)) return 0;
        if (Equals(item, Width)) return 1;
        if (Equals(item, Height)) return 2;
        return -1;
    }

    public bool Equals(GeneSequence? other)
    {
        return other != null && Length.Equals(other.Length) && Width.Equals(other.Width) && Height.Equals(other.Height);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            GeneSequence geneSequence => Equals(geneSequence),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Length.GetHashCode(), Width.GetHashCode(), Height.GetHashCode());
    }
}



public class Gene(int value) : IEquatable<Gene>
{
    public int Value { get; } = value;

    public bool Equals(Gene? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Gene g && Equals(g);
    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
    public static implicit operator Gene(int value) => new Gene(value);
}
