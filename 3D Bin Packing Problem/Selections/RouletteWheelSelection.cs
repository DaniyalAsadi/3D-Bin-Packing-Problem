namespace _3D_Bin_Packing_Problem.Selections;
internal class RouletteWheelSelection
{
    public static T Select<T>(List<T> items, Func<T, double> getFitness)
        where T : notnull
    {
        double totalFitness = items.Sum(getFitness);
        double randomValue = new Random().NextDouble() * totalFitness;

        double cumulativeFitness = 0.0;
        foreach (var item in items)
        {
            cumulativeFitness += getFitness(item);
            if (cumulativeFitness >= randomValue)
            {
                return item;
            }
        }

        throw new Exception();
    }
}
internal static class RouletteWheelSelectionExtensions
{
    public static T PerformRouletteWheelSelection<T>(this List<T> items, Func<T, double> getFitness)
        where T : notnull
    {
        return RouletteWheelSelection.Select(items, getFitness);
    }
}