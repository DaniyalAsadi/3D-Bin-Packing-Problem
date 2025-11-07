using _3D_Bin_Packing_Problem.Core.Model;

public class BenchmarkGenerator
{
    private static readonly Random _random = new Random(42); // Seed ثابت برای نتایج قابل تکرار

    public static List<Item> GenerateBenchmarkItems(ItemClass itemClass, int itemCount)
    {
        var items = new List<Item>();
        var classInfo = GetClassInfo(itemClass);

        for (int i = 0; i < itemCount; i++)
        {
            int length = _random.Next(classInfo.LengthRange.Item1, classInfo.LengthRange.Item2 + 1);
            int width = _random.Next(classInfo.WidthRange.Item1, classInfo.WidthRange.Item2 + 1);
            int height = _random.Next(classInfo.HeightRange.Item1, classInfo.HeightRange.Item2 + 1);

            items.Add(new Item(length, width, height));
        }

        return items;
    }

    public static List<BinType> GenerateBenchmarkBins(ItemClass itemClass)
    {
        var bins = new List<BinType>
        {
            new() { Description = "Size 1", Length = 150, Width = 100, Height = 100, CostFunc = () => 63800 },
            new() { Description = "Size 2", Length = 200, Width = 150, Height = 100, CostFunc = () => 115500 },
            new() { Description = "Size 3", Length = 200, Width = 200, Height = 150, CostFunc = () => 172700 },
            new() { Description = "Size 4", Length = 300, Width = 200, Height = 200, CostFunc = () => 247500 },
            new() { Description = "Size 5", Length = 350, Width = 250, Height = 200, CostFunc = () => 446600 },
            new() { Description = "Size 6", Length = 450, Width = 250, Height = 200, CostFunc = () => 559900 },
            new() { Description = "Size 7", Length = 400, Width = 300, Height = 250, CostFunc = () => 686400 },
            new() { Description = "Size 8", Length = 450, Width = 400, Height = 300, CostFunc = () => 1043900 },
            new() { Description = "Size 9", Length = 550, Width = 450, Height = 350, CostFunc = () => 1375000 },
        };

        return bins;
    }

    public static BenchmarkSuite GenerateCompleteBenchmark()
    {
        var benchmark = new BenchmarkSuite();

        // تولید برای هر 8 کلاس
        foreach (ItemClass itemClass in Enum.GetValues(typeof(ItemClass)))
        {
            var classBenchmark = new ClassBenchmark
            {
                ItemClass = itemClass,
                ClassInfo = GetClassInfo(itemClass)
            };

            // 4 گروه با تعداد آیتم‌های مختلف: 50, 100, 150, 200
            int[] itemCounts = { 50, 100, 150, 200 };
            foreach (int count in itemCounts)
            {
                // 10 نمونه برای هر گروه (مطابق مقاله)
                for (int instance = 1; instance <= 10; instance++)
                {
                    var benchmarkInstance = new BenchmarkInstance
                    {
                        InstanceName = $"Class_{(int)itemClass + 1}_{count}_{instance}",
                        ItemClass = itemClass,
                        ItemCount = count,
                        InstanceNumber = instance,
                        Items = GenerateBenchmarkItems(itemClass, count),
                        Bins = GenerateBenchmarkBins(itemClass)
                    };

                    classBenchmark.Instances.Add(benchmarkInstance);
                }
            }

            benchmark.Classes.Add(classBenchmark);
        }

        return benchmark;
    }

    private static ClassInfo GetClassInfo(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Class1 => new ClassInfo
            {
                BinWidth = 100,
                BinHeight = 100,
                BinLength = 100,
                LengthRange = Tuple.Create(1, 50),   // [1, 1/2L]
                WidthRange = Tuple.Create(1, 50),    // [1, 1/2W]
                HeightRange = Tuple.Create(67, 100)  // [2/3H, H]
            },
            ItemClass.Class2 => new ClassInfo
            {
                BinWidth = 100,
                BinHeight = 100,
                BinLength = 100,
                LengthRange = Tuple.Create(67, 100), // [2/3L, L]
                WidthRange = Tuple.Create(67, 100),  // [2/3W, W]
                HeightRange = Tuple.Create(1, 50)    // [1, 1/2H]
            },
            ItemClass.Class3 => new ClassInfo
            {
                BinWidth = 100,
                BinHeight = 100,
                BinLength = 100,
                LengthRange = Tuple.Create(67, 100), // [2/3L, L]
                WidthRange = Tuple.Create(67, 100),  // [2/3W, W]
                HeightRange = Tuple.Create(67, 100)  // [2/3H, H]
            },
            ItemClass.Class4 => new ClassInfo
            {
                BinWidth = 100,
                BinHeight = 100,
                BinLength = 100,
                LengthRange = Tuple.Create(50, 100), // [1/2L, L]
                WidthRange = Tuple.Create(50, 100),  // [1/2W, W]
                HeightRange = Tuple.Create(50, 100)  // [1/2H, H]
            },
            ItemClass.Class5 => new ClassInfo
            {
                BinWidth = 100,
                BinHeight = 100,
                BinLength = 100,
                LengthRange = Tuple.Create(1, 50),  // [1, 1/2L]
                WidthRange = Tuple.Create(1, 50),   // [1, 1/2W]
                HeightRange = Tuple.Create(1, 50)   // [1, 1/2H]
            },
            ItemClass.Class6 => new ClassInfo
            {
                BinWidth = 10,
                BinHeight = 10,
                BinLength = 10,
                LengthRange = Tuple.Create(1, 10), // [1, 10]
                WidthRange = Tuple.Create(1, 10),  // [1, 10]
                HeightRange = Tuple.Create(1, 10)  // [1, 10]
            },
            ItemClass.Class7 => new ClassInfo
            {
                BinWidth = 40,
                BinHeight = 40,
                BinLength = 40,
                LengthRange = Tuple.Create(1, 35), // [1, 35]
                WidthRange = Tuple.Create(1, 35),  // [1, 35]
                HeightRange = Tuple.Create(1, 35)  // [1, 35]
            },
            ItemClass.Class8 => new ClassInfo
            {
                BinWidth = 100,
                BinHeight = 100,
                BinLength = 100,
                LengthRange = Tuple.Create(1, 100), // [1, 100]
                WidthRange = Tuple.Create(1, 100),  // [1, 100]
                HeightRange = Tuple.Create(1, 100)  // [1, 100]
            },
            _ => throw new ArgumentException($"Unknown item class: {itemClass}")
        };
    }
}