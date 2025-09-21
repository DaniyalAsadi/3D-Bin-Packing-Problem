namespace _3D_Bin_Packing_Problem.Model;

public class Gene(int value) : IEquatable<Gene>
{
    private int _value = value;
    public int Value => _value;
    public void SetValue(int value) => _value = value;
    public bool Equals(Gene? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Gene g && Equals(g);
    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
    public static implicit operator Gene(int value) => new Gene(value);
}