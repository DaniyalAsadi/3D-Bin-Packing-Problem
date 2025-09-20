using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.Services.PopulationGenerator.Implementation;
public class PopulationGenerator()
    : IPopulationGenerator
{
    private readonly Random _random = new Random();

    // تولید یک GeneSequence (یک جعبه تصادفی)
    private GeneSequence CreateRandomGeneSequence(int minLength, int maxLength, int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        var length = new Gene(_random.Next(minLength, maxLength + 1));
        var width = new Gene(_random.Next(minWidth, maxWidth + 1));
        var height = new Gene(_random.Next(minHeight, maxHeight + 1));

        var box = new Box
        {
            Length = length.Value,
            Width = width.Value,
            Height = height.Value,
            Price = 0 // قیمت می‌تونه بعداً بر اساس فرمول مقاله محاسبه بشه
        };

        return new GeneSequence(box);
    }

    // تولید یک کروموزوم
    private Chromosome CreateRandomChromosome(int binTypeCount, int minLength, int maxLength, int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        var geneSequences = new List<GeneSequence>();

        for (int i = 0; i < binTypeCount; i++)
        {
            geneSequences.Add(CreateRandomGeneSequence(minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight));
        }

        return new Chromosome(geneSequences);
    }

    // تولید جمعیت اولیه
    public List<Chromosome> Generate(List<Product> itemList, int populationSize, int binTypeCount)
    {
        // محاسبه کمترین و بیشترین طول، عرض و ارتفاع بین محصولات
        int minLength = itemList.Min(e => e.Length);
        int maxLength = itemList.Max(e => e.Length);

        int minWidth = itemList.Min(e => e.Width);
        int maxWidth = itemList.Max(e => e.Width);

        int minHeight = itemList.Min(e => e.Height);
        int maxHeight = itemList.Max(e => e.Height);

        var population = new List<Chromosome>();

        // طبق مقاله: 2 × PopulationSize ساخته می‌شود و بعد بهترین‌ها انتخاب می‌شوند
        for (int i = 0; i < 2 * populationSize; i++)
        {
            var chromosome = CreateRandomChromosome(binTypeCount, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight);
            population.Add(chromosome);
        }

        // در اینجا باید Fitness محاسبه شود و بعد بهترین PopulationSize انتخاب شوند
        // فعلاً به صورت ساده همه را برمی‌گردانیم
        return population.Take(populationSize).ToList();
    }
}

