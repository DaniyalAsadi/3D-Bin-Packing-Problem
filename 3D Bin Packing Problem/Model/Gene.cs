namespace _3D_Bin_Packing_Problem.Model;

public class Gene(int value) : IEquatable<Gene>
{
    public int Value { get; private set; } = value;

    public void SetValue(int value) => Value = value;

    public bool Equals(Gene? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is Gene g && Equals(g);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static implicit operator Gene(int value) => new(value);

    /// <summary>
    /// Clone method for deep copy
    /// </summary>
    public Gene Clone() => new(Value);
}
