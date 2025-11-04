namespace _3D_Bin_Packing_Problem.Core.Configuration;
public static class AppConstants
{
    public const string AlgorithmName = "Two-Layer Genetic Packing";
    public const string Author = "Daniyal - M.Sc. Thesis";

    // Example of predefined heuristic identifiers
    public const string OuterLayerHeuristic = "GA";
    public const string InnerLayerHeuristic = "MultiHeuristic";

    // Default precision or rounding factor
    public const float Tolerance = 1e-6f;
}
public class GeneticAlgorithmSettings
{
    public int PopulationSize { get; set; } = 100;
    public double MutationRate { get; set; } = 0.02;
    public double CrossoverRate { get; set; } = 0.8;
    public int MaxIteration { get; set; } = 500;
    public int TournamentGroupSize { get; set; } = 5;
    public int ElitismPopulationSize { get; set; } = 5;

    // ✅ نسبت حداقل تکیه‌گاه
    public double SupportThreshold { get; set; } = 0.75;

    // ✅ ضرایب وزن‌دهی در تابع برازندگی
    public double AlphaWeight { get; set; } = 1.0;
    public double BetaWeight { get; set; } = 1.0;

    // ✅ ضریب جریمه برای آیتم‌های بسته‌نشده
    public int PenaltyCoefficient { get; set; } = 200000;
}
public class PackingSettings
{

    // اتصال تنظیمات الگوریتم ژنتیک
    public GeneticAlgorithmSettings Genetic { get; set; } = new GeneticAlgorithmSettings();
}
public static class SettingsManager
{
    private static PackingSettings _settings = new PackingSettings();
    public static PackingSettings Current => _settings;

    public static void Initialize(PackingSettings settings)
    {
        _settings = settings ?? new PackingSettings();
    }
}