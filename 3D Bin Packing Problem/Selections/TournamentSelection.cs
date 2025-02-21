namespace _3D_Bin_Packing_Problem.Selections;

internal class TournamentSelection
{
    private readonly Random _random;

    public TournamentSelection()
    {
        _random = new Random();
    }

    public T Select<T>(List<T> population, Func<T, double> fitnessFunction, int tournamentSize)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population cannot be null or empty.", nameof(population));
        if (tournamentSize <= 0)
            throw new ArgumentException("Tournament size must be greater than zero.", nameof(tournamentSize));

        var tournament = new List<T>();
        for (int i = 0; i < tournamentSize; i++)
        {
            int randomIndex = _random.Next(population.Count);
            tournament.Add(population[randomIndex]);
        }

        return tournament.OrderByDescending(fitnessFunction).First();
    }
}
public static class TournamentSelectionExtensions
{
    public static T PerformTournamentSelection<T>(this List<T> population, Func<T, double> fitnessFunction, int tournamentSize)
    {
        var tournamentSelection = new TournamentSelection();
        return tournamentSelection.Select(population, fitnessFunction, tournamentSize);
    }
}