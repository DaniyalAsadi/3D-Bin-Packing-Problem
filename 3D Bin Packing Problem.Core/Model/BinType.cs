using Ardalis.GuardClauses;
using System;

namespace _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Describes a bin's dimensional properties and derived cost used within packing evaluations.
/// </summary>
public class BinType
{
    /// <summary>
    /// Describes a bin's dimensional properties and derived cost used within packing evaluations.
    /// </summary>
    internal BinType(string name,
        float length,
        float width,
        float height)
    {
        Name = name;
        InnerDimensions = new Dimensions(length, width, height);
    }

    /// <summary>
    /// شناسه منحصربه‌فرد
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
    /// <summary>
    /// نام نوع کانتینر (e.g., "20ft Dry")
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// ابعاد داخلی قابل استفاده (mm)
    /// </summary>
    public Dimensions InnerDimensions { get; }

    /// <summary>
    /// حجم Bin فعلی
    /// </summary>
    public float Volume => InnerDimensions.Length * InnerDimensions.Width * InnerDimensions.Height;
    /// <summary>
    /// حداکثر وزن قابل تحمل (kg)
    /// </summary>
    public decimal MaxWeight { get; private set; }

    /// <summary>
    /// وزن خالی کانتینر (kg)
    /// </summary>
    public decimal TareWeight { get; private set; }

    /// <summary>
    /// مقدار هزینه
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    /// تبدیل ضمنی به SubBin
    /// </summary>
    public static implicit operator SubBin(BinType binType) => new SubBin(binType);

    /// <summary>
    /// ایجاد نسخه‌ی کپی از BinType
    /// </summary>
    public BinType Clone()
    {
        return new BinType(Name, InnerDimensions.Length, InnerDimensions.Width, InnerDimensions.Height)
        {
            MaxWeight = MaxWeight,
            Cost = Cost,
            TareWeight = TareWeight,

        };
    }
    public static BinType Create(
        string name,
        float length,
        float width,
        float height,
        decimal maxWeight,
        decimal cost,
        decimal tareWeight = 0)
    {
        name = Guard.Against.NullOrWhiteSpace(name);
        length = Guard.Against.NegativeOrZero(length, nameof(length));
        width = Guard.Against.NegativeOrZero(width, nameof(width));
        height = Guard.Against.NegativeOrZero(height, nameof(height));
        maxWeight = Guard.Against.NegativeOrZero(maxWeight, nameof(maxWeight));
        cost = Guard.Against.Negative(cost, nameof(cost));
        tareWeight = Guard.Against.Negative(tareWeight, nameof(tareWeight));

        return new BinType(name, length, width, height)
        {
            MaxWeight = maxWeight,
            Cost = cost,
            TareWeight = tareWeight,

        };
    }



    public override string ToString()
    {
        return $"Bin [{Name ?? "Unnamed"}: L×W×H=({InnerDimensions.Length}×{InnerDimensions.Width}×{InnerDimensions.Height}), Vol={Volume}, Cost={Cost:F2}]";
    }
}
