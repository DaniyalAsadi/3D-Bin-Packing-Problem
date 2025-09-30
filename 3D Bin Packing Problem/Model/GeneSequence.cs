using System.Collections;

namespace _3D_Bin_Packing_Problem.Model;

public class GeneSequence(BinType binType) : IEnumerable<Gene>, IEquatable<GeneSequence>
{
    public BinType BinType { get; private set; } = binType;
    public Gene Length { get; private set; } = binType.Length;
    public Gene Width { get; private set; } = binType.Width;
    public Gene Height { get; private set; } = binType.Height;

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