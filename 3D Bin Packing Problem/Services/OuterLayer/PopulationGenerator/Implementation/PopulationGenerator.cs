using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.OuterLayer.PopulationGenerator.Implementation;
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

        for (int i = 0; i < binTypeCount; i++)
        {
            geneSequences.Add(CreateRandomGeneSequence(
                minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight));
        }

        return new Chromosome(geneSequences);
    }

    // تولید جمعیت اولیه
    public List<Chromosome> Generate(List<Item> itemList, int populationSize, int binTypeCount)
    {
        // حداقل جعبه باید بزرگ‌ترین آیتم رو جا بده
        int minLength = itemList.Max(e => e.Length);
        int minWidth = itemList.Max(e => e.Width);
        int minHeight = itemList.Max(e => e.Height);

        // حداکثر رو می‌تونیم مجموع آیتم‌ها در نظر بگیریم (یا یک ضریب)
        int maxLength = itemList.Sum(e => e.Length);
        int maxWidth = itemList.Sum(e => e.Width);
        int maxHeight = itemList.Sum(e => e.Height);

        var population = new List<Chromosome>();

        for (int i = 0; i < 2 * populationSize; i++)
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

