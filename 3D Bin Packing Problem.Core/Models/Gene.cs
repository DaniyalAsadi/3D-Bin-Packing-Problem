using System;
using System.Globalization;

namespace _3D_Bin_Packing_Problem.Core.Models;

/// <summary>
/// Encapsulates a mutable integer gene value used within chromosomes.
/// </summary>
public class Gene(int value) : IEquatable<Gene>
{
    public int Value { get; } = value;


    public bool Equals(Gene? other) => other is not null && Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Gene g && Equals(g);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static implicit operator Gene(int value) => new(value);

    /// <summary>
    /// Clone method for deep copy
    /// </summary>
    public Gene Clone() => new(Value);
}
