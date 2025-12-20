using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Core.ViewModels;

/// <summary>
/// Collects the detailed results of packing a single bin, including packed items and remaining sub-bins.
/// </summary>
public class PackingResultViewModel
{
    /// <summary>
    /// آیتم‌هایی که با موفقیت در Bin قرار گرفتند
    /// </summary>
    public List<PackedItemViewModel> PackedItems { get; set; } = new();

    /// <summary>
    /// لیست Sub-binهای باقی‌مانده بعد از آخرین مرحله SUA
    /// </summary>
    public List<SubBinViewModel> RemainingSubBins { get; set; } = new();

    /// <summary>
    /// آیتم‌هایی که در Bin جا نشدند
    /// </summary>
    public List<ItemViewModel> LeftItems { get; set; } = new();
}

/// <summary>
/// نمایش یک آیتم که در Bin بسته‌بندی شده
/// </summary>
public record PackedItemViewModel
{
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public Guid BinTypeId { get; set; }

    public Guid InstanceId { get; set; }
    // موقعیت قرارگیری (گوشه پایین-چپ-پشت)
    public Vector3 Position { get; set; }

    // اورینتیشن آیتم در لحظه قرارگیری
    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Volume => Length * Width * Height;
    // نسبت تکیه‌گاه
    public double SupportRatio { get; set; }

    public override string ToString()
    {
        return $"Item ID: {ItemId}, Bin Type ID: {BinTypeId}, InstanceId: {InstanceId}, Position: ({Position.X}, {Position.Y}, {Position.Z}), Dimensions: {Length} x {Width} x {Height}, Support Ratio: {SupportRatio:P2}";
    }
}

/// <summary>
/// نمایش Sub-bin باقی‌مانده
/// </summary>
public class SubBinViewModel
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public override string ToString()
    {
        return $"SubBin Position: ({X}, {Y}, {Z}), Dimensions: {Length} x {Width} x {Height}";
    }
}

/// <summary>
/// نمایش یک آیتم جا نشده
/// </summary>
public class ItemViewModel
{
    public Guid Id { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public override string ToString()
    {
        return $"Item ID: {Id}, Dimensions: {Length} x {Width} x {Height}";
    }
}
