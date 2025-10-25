using System.Collections;

namespace _3D_Bin_Packing_Problem.Model;

/// <summary>
/// Represents a collection of genes describing a single bin type's dimensions.
/// </summary>
public class GeneSequence : IEnumerable<Gene>, IEquatable<GeneSequence>
{
    private static readonly Random Random = new();

    public BinType BinType { get; private set; }
    public Gene Length { get; private set; }
    public Gene Width { get; private set; }
    public Gene Height { get; private set; }

    public GeneSequence(BinType binType)
    {
        BinType = binType;
        Length = new Gene(binType.Length);
        Width = new Gene(binType.Width);
        Height = new Gene(binType.Height);
    }

    private IEnumerable<Gene> Items
    {
        get
        {
            yield return Length;
            yield return Width;
            yield return Height;
        }
    }

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
        return other is not null &&
               Length.Equals(other.Length) &&
               Width.Equals(other.Width) &&
               Height.Equals(other.Height);
    }

    public override bool Equals(object? obj) => obj is GeneSequence seq && Equals(seq);

    public override int GetHashCode() =>
        HashCode.Combine(Length.GetHashCode(), Width.GetHashCode(), Height.GetHashCode());

    public override string ToString() => BinType.ToString();

    /// <summary>
    /// Deep clone for GeneSequence including BinType
    /// </summary>
    public GeneSequence Clone()
    {
        var clonedBin = BinType.Clone();
        return new GeneSequence(clonedBin);
    }

    /// <summary>
    /// Apply a random rotation (permutes L, W, H)
    /// </summary>
    public void ApplyRandomRotation()
    {
        var orientation = Random.Next(6);
        var l = Length.Clone();
        var w = Width.Clone();
        var h = Height.Clone();

        switch (orientation)
        {
            case 0: // (L, W, H)
                (Length, Width, Height) = (l, w, h);
                break;
            case 1: // (L, H, W)
                (Length, Width, Height) = (l, h, w);
                break;
            case 2: // (W, L, H)
                (Length, Width, Height) = (w, l, h);
                break;
            case 3: // (W, H, L)
                (Length, Width, Height) = (w, h, l);
                break;
            case 4: // (H, L, W)
                (Length, Width, Height) = (h, l, w);
                break;
            case 5: // (H, W, L)
                (Length, Width, Height) = (h, w, l);
                break;
        }
    }
}
