using Ardalis.GuardClauses;
using System;

namespace _3D_Bin_Packing_Problem.Core.Guards;
public static class ItemGuards
{
    /// <summary>
    /// Fragile items cannot be stackable.
    /// </summary>
    public static void FragileItemCannotBeStackable(
        this IGuardClause clause,
        bool isFragile,
        bool isStackable,
        string? parameterName = null)
    {
        if (isFragile && isStackable)
        {
            throw new ArgumentException(
                "Fragile items cannot be stackable.",
                parameterName ?? $"{nameof(isFragile)}-{nameof(isStackable)}");
        }
    }

    /// <summary>
    /// MaxLoadOnTop is only allowed when item is stackable.
    /// </summary>
    public static void InvalidMaxLoadForNonStackable(
        this IGuardClause clause,
        bool isStackable,
        decimal? maxLoadOnTop,
        string? parameterName = null)
    {
        if (maxLoadOnTop.HasValue && !isStackable)
        {
            throw new ArgumentException(
                "MaxLoadOnTop must be null when item is not stackable.",
                parameterName ?? $"{nameof(isStackable)}-{nameof(maxLoadOnTop)}");
        }
    }
}
