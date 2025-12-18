using _3D_Bin_Packing_Problem.Core.Guards;
using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;


namespace _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Represents an item with dimensions and volume information for packing.
/// </summary>
public class Item
{
    private Item(Dimensions dimensions)
    {
        Dimensions = dimensions;
    }
    internal Item(
        float length,
        float width,
        float height)
    {
        length = Guard.Against.NegativeOrZero(length, nameof(length));
        width = Guard.Against.NegativeOrZero(width, nameof(width));
        height = Guard.Against.NegativeOrZero(height, nameof(height));
        Dimensions = new Dimensions(length, width, height);
    }
    /// <summary>
    /// شناسه منحصربه‌فرد قلم
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();
    /// <summary>
    /// طول، عرض، ارتفاع (Length, Width, Height) در واحد میلی‌متر
    /// </summary>
    public Dimensions Dimensions { get; private set; }
    /// <summary>
    /// وزن قلم (kg)
    /// </summary>
    public float Weight { get; private set; }
    /// <summary>
    /// حجم محاسبه‌شده (Dimensions.Volume) – read-only
    /// </summary>
    public float Volume => Dimensions.Width * Dimensions.Height * Dimensions.Length;
    /// <summary>
    /// لیست جهت‌های مجاز چرخش (از ۶ جهت ممکن: xy, xz, yx, yz, zx, zy)
    /// </summary>
    public List<Orientation> Orientations { get; private set; } =
        [Orientation.Xy, Orientation.Xz, Orientation.Yx, Orientation.Yz, Orientation.Zx, Orientation.Zy];

    /// <summary>
    /// آیا قلم شکننده است؟ (نیاز به قرارگیری ایمن)
    /// </summary>
    public bool IsFragile { get; private set; } = false;
    /// <summary>
    /// آیا می‌توان قلم دیگری روی آن قرار داد؟
    /// </summary>
    public bool IsStackable { get; private set; } = true;
    /// <summary>
    /// حداکثر وزن قابل تحمل روی این قلم (kg) – اگر Stackable=false، null
    /// </summary>
    public decimal? MaxLoadOnTop { get; private set; } = null;

    /// <summary>
    /// اولویت چیدمان (بالاتر = زودتر چیده شود)
    /// </summary>
    public int Priority { get; private set; } = 0;

    /// <summary>
    /// ترتیب اجباری بارگیری (برای رعایت FIFO/LIFO در سفارش)
    /// </summary>
    public int? LoadingSequence { get; private set; } = null;
    /// <summary>
    /// نوع بار: Normal, Hazardous, Liquid, Refrigerated, etc.
    /// </summary>
    public LoadType LoadType { get; private set; } = LoadType.Normal;
    /// <summary>
    /// ارجاع به شناسه سفارش (برای traceability)
    /// </summary>
    public Guid OrderId { get; private set; }

    public static Item Create(
        float length,
        float width,
        float height,
        float weight,
        Guid orderId,
        List<Orientation>? orientations = null,
        bool isFragile = false,
        bool isStackable = true,
        decimal? maxLoadOnTop = null,
        int priority = 0,
        int? loadingSequence = null,
        LoadType loadType = LoadType.Normal)
    {
        length = Guard.Against.NegativeOrZero(length, nameof(length));
        width = Guard.Against.NegativeOrZero(width, nameof(width));
        height = Guard.Against.NegativeOrZero(height, nameof(height));
        weight = Guard.Against.NegativeOrZero(weight, nameof(weight));
        orientations ??= [Orientation.Xy, Orientation.Xz, Orientation.Yx, Orientation.Yz, Orientation.Zx, Orientation.Zy];
        Guard.Against.InvalidMaxLoadForNonStackable(isStackable, maxLoadOnTop);
        Guard.Against.FragileItemCannotBeStackable(isFragile, isStackable);
        priority = Guard.Against.Negative(priority, nameof(priority));
        return new Item(new Dimensions(length, width, height))
        {
            Weight = weight,
            OrderId = orderId,
            Orientations = orientations,
            IsFragile = isFragile,
            IsStackable = isStackable,
            MaxLoadOnTop = maxLoadOnTop,
            Priority = priority,
            LoadingSequence = loadingSequence,
            LoadType = loadType
        };

    }


    public override string ToString()
    {
        return $"Length: {Dimensions.Length}, Width: {Dimensions.Width}, Height: {Dimensions.Height}";
    }
    public float GetMinimumDimension()
    {
        return Math.Min(Dimensions.Length, Math.Min(Dimensions.Width, Dimensions.Height));
    }
}
