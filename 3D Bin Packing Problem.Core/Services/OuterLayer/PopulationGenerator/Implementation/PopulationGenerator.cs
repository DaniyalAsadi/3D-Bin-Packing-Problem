using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Services.OuterLayer.PopulationGenerator.Implementation;

/// <summary>
/// Generates initial populations of chromosomes using randomly sampled bin dimensions within computed bounds.
/// </summary>
public class PopulationGenerator : IPopulationGenerator
{
    private readonly Random _random = new Random();

    private List<BinType> _availableBinTypes = [];

    public void SetAvailableBins(List<BinType> predefinedBinTypes)
    {
        _availableBinTypes = predefinedBinTypes
            ?? throw new ArgumentNullException(nameof(predefinedBinTypes));

        if (!_availableBinTypes.Any())
            throw new ArgumentException("At least one predefined bin type is required.", nameof(predefinedBinTypes));
    }

    // تولید یک GeneSequence از بین bin‌های از پیش تعریف‌شده
    private GeneSequence CreateRandomGeneSequence()
    {
        var randomBin = _availableBinTypes[_random.Next(_availableBinTypes.Count)];
        var clone = randomBin.Clone(); // تا از تغییر اشتباهی روی لیست اصلی جلوگیری بشه
        return new GeneSequence(clone);
    }

    // تولید یک کروموزوم با ترکیب تصادفی از BinTypeهای از پیش تعریف‌شده
    private Chromosome CreateRandomChromosome(int binTypeCount)
    {
        var geneSequences = new List<GeneSequence>();

        for (var i = 0; i < binTypeCount; i++)
            geneSequences.Add(CreateRandomGeneSequence());

        return new Chromosome(geneSequences);
    }

    // تولید جمعیت اولیه
    public List<Chromosome> Generate(List<Item> itemList, int populationSize, int binTypeCount)
    {
        if (_availableBinTypes == null || _availableBinTypes.Count == 0)
            throw new InvalidOperationException("No predefined bin types provided for population generation.");

        var population = new List<Chromosome>();

        // ۲ برابر تولید برای تنوع بیشتر
        for (var i = 0; i < 2 * populationSize; i++)
        {
            var chromosome = CreateRandomChromosome(binTypeCount);
            population.Add(chromosome);
        }

        // 🔹 ارزیابی اولیه بر اساس مجموع هزینه جعبه‌ها
        population = population
            .OrderBy(c => c.GeneSequences.Sum(g => g.BinType.Cost))
            .Take(populationSize)
            .ToList();

        return population;
    }
}
