using System;

namespace _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Describes a bin's dimensional properties and derived cost used within packing evaluations.
/// </summary>
public class BinType
{
    private Func<double>? _valueFunc;
    private double? _cachedCost;

    private int _length;
    private int _width;
    private int _height;

    public Guid Id { get; } = Guid.NewGuid();
    public string? Description { get; set; }

    public int Length
    {
        get => _length;
        set
        {
            if (_length != value)
            {
                _length = value;
                _cachedCost = null; // invalidate cache automatically
            }
        }
    }

    public int Width
    {
        get => _width;
        set
        {
            if (_width != value)
            {
                _width = value;
                _cachedCost = null;
            }
        }
    }

    public int Height
    {
        get => _height;
        set
        {
            if (_height != value)
            {
                _height = value;
                _cachedCost = null;
            }
        }
    }

    /// <summary>
    /// حجم Bin فعلی
    /// </summary>
    public int Volume => Length * Width * Height;

    /// <summary>
    /// هزینه Bin بر اساس فرمول مقاله Alvarez-Valdés et al. (2013)
    /// Ct = 10000 × (1.2Vt − 0.2LWH) / (LWH)
    /// فقط یک بار محاسبه شده و کش می‌شود تا در دفعات بعدی سریع‌تر برگردد
    /// </summary>
    public double Cost
    {
        get
        {
            if (_cachedCost.HasValue)
                return _cachedCost.Value;

            if (_valueFunc is null)
                return 0.0;

            _cachedCost = _valueFunc();
            return _cachedCost.Value;
        }
    }

    /// <summary>
    /// تابع محاسبه هزینه؛ تنظیم آن باعث پاک شدن کش می‌شود
    /// </summary>
    public Func<double> CostFunc
    {
        set
        {
            _valueFunc = value;
            _cachedCost = null; // invalidate cache when cost formula changes
        }
    }

    /// <summary>
    /// تبدیل ضمنی به SubBin
    /// </summary>
    public static implicit operator SubBin(BinType binType) => new SubBin(binType);

    /// <summary>
    /// ایجاد نسخه‌ی کپی از BinType (همراه با کش)
    /// </summary>
    public BinType Clone()
    {
        return new BinType
        {
            Length = Length,
            Width = Width,
            Height = Height,
            Description = Description,
            _valueFunc = _valueFunc,
            _cachedCost = _cachedCost
        };
    }

    public override string ToString()
    {
        return $"Bin [{Description ?? "Unnamed"}: L×W×H=({Length}×{Width}×{Height}), Vol={Volume}, Cost={Cost:F2}]";
    }
}
