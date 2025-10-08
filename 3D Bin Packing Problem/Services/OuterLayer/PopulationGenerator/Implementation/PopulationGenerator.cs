using _3D_Bin_Packing_Problem.Helpers;
using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.PopulationGenerator.Implementation;

/// <summary>
/// Generates initial populations of chromosomes using randomly sampled bin dimensions within computed bounds.
/// </summary>
public class PopulationGenerator() : IPopulationGenerator
{
    private readonly Random _random = new Random();



    // تولید یک GeneSequence (یک جعبه تصادفی)
    private GeneSequence CreateRandomGeneSequence(
        int minLength, int maxLength,
        int minWidth, int maxWidth,
        int minHeight, int maxHeight)
    {
        var length = new Gene(_random.Next(minLength, maxLength + 1));
        var width = new Gene(_random.Next(minWidth, maxWidth + 1));
        var height = new Gene(_random.Next(minHeight, maxHeight + 1));

        var box = new BinType
        {
            Length = length.Value,
            Width = width.Value,
            Height = height.Value
        };


        // قیمت جعبه بر اساس حجم

        return new GeneSequence(box);
    }

    // تولید یک کروموزوم
    private Chromosome CreateRandomChromosome(
        int binTypeCount,
        int minLength, int maxLength,
        int minWidth, int maxWidth,
        int minHeight, int maxHeight)
    {
        var geneSequences = new List<GeneSequence>();

        for (var i = 0; i < binTypeCount; i++)
        {
            geneSequences.Add(CreateRandomGeneSequence(
                minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight));
        }

        return new Chromosome(geneSequences);
    }

    // تولید جمعیت اولیه
    public List<Chromosome> Generate(List<Item> itemList, int populationSize, int binTypeCount)
    {
        var b = BinDimensionBounds.Compute(itemList);

        // حالا می‌تونی بازهٔ تصادفی‌سازی رو از این‌ها بگیری
        var minLength = b.MinLength;
        var minWidth = b.MinWidth;
        var minHeight = b.MinHeight;

        var maxLength = b.MaxLength;
        var maxWidth = b.MaxWidth;
        var maxHeight = b.MaxHeight;


        var population = new List<Chromosome>();

        for (var i = 0; i < 2 * populationSize; i++)
        {
            var chromosome = CreateRandomChromosome(binTypeCount, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight);
            population.Add(chromosome);
        }

        // 🔹 اینجا باید Fitness واقعی هر کروموزوم محاسبه بشه
        // فعلا ساده: جمع هزینه جعبه‌های هر کروموزوم
        population = population
            .OrderBy(c => c.GeneSequences.Sum(g => g.BinType.Cost))
            .Take(populationSize)
            .ToList();

        return population;
    }
}

