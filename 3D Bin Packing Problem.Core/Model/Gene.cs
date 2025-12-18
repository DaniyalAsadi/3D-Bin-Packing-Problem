using _3D_Bin_Packing_Problem.Core.Configuration;
using System;
using System.Globalization;

namespace _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Encapsulates a mutable integer gene value used within chromosomes.
/// </summary>
public class Gene(float value) : IEquatable<Gene>
{
    public float Value { get; } = value;


    public bool Equals(Gene? other) => other is not null && Math.Abs(Value - other.Value) < AppConstants.Tolerance;

    public override bool Equals(object? obj) => obj is Gene g && Equals(g);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static implicit operator Gene(int value) => new(value);

    /// <summary>
    /// Clone method for deep copy
    /// </summary>
    public Gene Clone() => new(Value);
}
