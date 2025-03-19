using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem;

public partial class Program
{
    public static double GetUserInput(string paramName, string description, double defaultValue, double min, double max)
    {
        Console.WriteLine($"{paramName}: {description}");
        Console.Write($"Enter a value ({min} - {max}, default {defaultValue}): ");

        string input = Console.ReadLine();
        if (double.TryParse(input, out double value))
        {
            return Math.Clamp(value, min, max); // Keep within valid range
        }

        return defaultValue;
    }

    static void Main(string[] args)
    {
        Random random = new Random();

        List<Product> products = new List<Product>()
        {
            new() { Id = 1, Length = 2, Width = 2, Height = 2 },
            new() { Id = 2, Length = 2, Width = 2, Height = 2 },
            new() { Id = 3, Length = 2, Width = 2, Height = 2 },
            new() { Id = 4, Length = 2, Width = 2, Height = 2 },
            new() { Id = 5, Length = 2, Width = 2, Height = 2 },
            new() { Id = 6, Length = 2, Width = 2, Height = 2 },
            new() { Id = 7, Length = 2, Width = 2, Height = 2 },
            new() { Id = 8, Length = 2, Width = 2, Height = 2 },
            new() { Id = 9, Length = 1, Width = 1, Height = 1 },
            //new() { Id = 10, Length = 1, Width = 1, Height = 1 },
            //new() { Id = 11, Length = 1, Width = 1, Height = 1 },
            //new() { Id = 10, Length = 1, Width = 1, Height = 1 },
            //new() { Id = 11, Length = 1, Width = 1, Height = 1 },
        };
        List<Box> boxes = new List<Box>()
        {
            new () { Id = 1, Length = 4, Width = 4, Height = 4, Price =  6000 },
            new () { Id = 2, Length = 1, Width = 1, Height = 1, Price =  1000 },
        };



        //List<Box> boxes = new()
        //{
        //    new() { Id = 1, Length = 15, Width = 10, Height = 10, Price = 3600},
        //    new() { Id = 2, Length = 20, Width = 15, Height = 10, Price = 6450},
        //    new() { Id = 3, Length = 20, Width = 20, Height = 15, Price = 8900},
        //    new() { Id = 4, Length = 30, Width = 20, Height = 20, Price = 12000},
        //    new() { Id = 5, Length = 35, Width = 25, Height = 20, Price = 16300},
        //    new() { Id = 6, Length = 45, Width = 25, Height = 20, Price = 18900},
        //    new() { Id = 7, Length = 40, Width = 30, Height = 25, Price = 22400},
        //    new() { Id = 8, Length = 45, Width = 40, Height = 30, Price = 55000},
        //    new() { Id = 9, Length = 55, Width = 45, Height = 35, Price = 80148},
        //};

        Console.WriteLine("Population: Initialized");


        chromosome.BoxPlacements.ToList().ForEach(Console.WriteLine);


    }
}
