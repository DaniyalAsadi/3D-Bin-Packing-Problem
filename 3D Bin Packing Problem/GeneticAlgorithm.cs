using _3D_Bin_Packing_Problem.CrossOvers;
using _3D_Bin_Packing_Problem.Models;
using _3D_Bin_Packing_Problem.Selections;

namespace _3D_Bin_Packing_Problem;
public class GeneticAlgorithm
{
    public List<Chromosome> Population { get; set; } = new List<Chromosome>();
    public List<Product> Products { get; set; }
    public List<Box> AvailableBoxes { get; set; }

    public GeneticHyperParameters HyperParameters { get; set; }

    private Random _rnd = new Random();
    private static readonly Orientation[] _orientations = (Orientation[])Enum.GetValues(typeof(Orientation));

    public GeneticAlgorithm(
        List<Product> products,
        List<Box> availableBoxes,
        double basePackingFactor,
        double maxPackingFactor,
        double baseCostFactor,
        double minCostFactor)
    {
        Products = products;
        AvailableBoxes = availableBoxes;
        HyperParameters = new GeneticHyperParameters(basePackingFactor, maxPackingFactor, baseCostFactor, minCostFactor);
        InitializePopulation();
    }
    public void SetHyperParameter(
        int populationSize,
        int generations,
        double mutationRate)
    {
        HyperParameters = new GeneticHyperParameters(populationSize, generations, mutationRate);
    }

    private void InitializePopulation()
    {
        for (int i = 0; i < HyperParameters.PopulationSize; i++)
        {
            var chromosome = new Chromosome();
            var shuffled = Products.OrderBy(x => x.Volume).ToList();

            foreach (var product in shuffled)
                chromosome.Genes.Add(new Gene(product, _orientations[_rnd.Next(_orientations.Length)]));

            chromosome.Fitness = EvaluateChromosome(chromosome, AvailableBoxes, out var boxes, 0, HyperParameters.Generations);
            chromosome.BoxPlacements = boxes;


            Population.Add(chromosome);
        }
    }


    private double EvaluateChromosome(Chromosome chromosome, List<Box> availableBoxes, out List<BoxPlacement> boxPlacements, int generation, int totalGenerations)
    {
        var placements = new List<BoxPlacement>();
        var boxDict = availableBoxes
            .OrderBy(b => b.Price)
            .ToDictionary(b => b, b => (b.Length, b.Width, b.Height));

        foreach (var gene in chromosome.Genes)
        {
            bool placed = false;
            var dims = OrientationHelper.GetDimensions(gene.Product, gene.Orientation);

            foreach (var placement in placements)
            {
                if (placement.TryPlaceProduct(gene))
                {
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                var viableBoxes = boxDict
                    .Where(b => dims.Length <= b.Value.Length && dims.Height <= b.Value.Height && dims.Width <= b.Value.Width)
                    .Take(HyperParameters.BoxSelectionLimit) // Take only top 3 cheap boxes
                    .Select(b => b.Key)
                    .ToList();

                if (viableBoxes.Count == 0)
                {
                    boxPlacements = placements.ToList();
                    return double.MaxValue;
                }

                var selectedBox = viableBoxes[_rnd.Next(viableBoxes.Count)];
                var newPlacement = new BoxPlacement(selectedBox);

                if (newPlacement.TryPlaceProduct(gene))
                {
                    placements.Add(newPlacement);
                }
                else
                {
                    boxPlacements = placements.ToList();
                    return double.MaxValue;
                }
            }
        }

        int totalCost = placements.Sum(p => p.Box.Price);
        double avgBoxPrice = availableBoxes.Average(b => b.Price);

        // Dynamic Scaling of α and β
        double baseAlpha = HyperParameters.FitnessFunctionWeights.BasePackingFactor, maxAlpha = HyperParameters.FitnessFunctionWeights.MaxPackingFactor;
        double baseBeta = HyperParameters.FitnessFunctionWeights.BaseCostFactor, minBeta = HyperParameters.FitnessFunctionWeights.MinCostFactor;

        double alpha = baseAlpha + ((double)generation / totalGenerations) * (maxAlpha - baseAlpha);
        double beta = baseBeta - ((double)generation / totalGenerations) * (baseBeta - minBeta);

        double normalizedCost = totalCost / avgBoxPrice;

        boxPlacements = placements.ToList();
        return alpha * placements.Count + beta * normalizedCost;
    }

    private void Mutation(Chromosome chromosome, int currentGeneration, int maxGenerations, double initialMutationRate)
    {
        Random _rnd = new Random();
        int size = chromosome.Genes.Count;

        // Adaptive mutation formula: decreases mutation rate over generations
        double mutationRate = initialMutationRate * (1 - (double)currentGeneration / maxGenerations);

        // Swap two genes with adaptive probability
        if (_rnd.NextDouble() < mutationRate)
        {
            int index1 = _rnd.Next(size);
            int index2 = _rnd.Next(size);
            (chromosome.Genes[index1], chromosome.Genes[index2]) = (chromosome.Genes[index2], chromosome.Genes[index1]);
        }

        // Change orientation with adaptive probability
        if (_rnd.NextDouble() < mutationRate)
        {
            int index = _rnd.Next(size);
            chromosome.Genes[index].Orientation = (Orientation)_rnd.Next(Enum.GetValues(typeof(Orientation)).Length);
        }
    }

    public Chromosome Run()
    {
        for (int gen = 0; gen < HyperParameters.Generations; gen++)
        {

            var newPopulation = new List<Chromosome>();

            for (int i = 0; i < HyperParameters.PopulationSize; i++)
            {
                var parent1 = Population.PerformTournamentSelection(c => c.Fitness, HyperParameters.PerformTournamentSelection);
                var parent2 = Population.PerformTournamentSelection(c => c.Fitness, HyperParameters.PerformTournamentSelection);

                var crossOver = CrossoverFactory.GetCrossover(HyperParameters.CrossoverType);
                var (child1, child2) = crossOver.CrossOver(parent1, parent2);
                // adaptive mutation
                Mutation(child1, gen, HyperParameters.Generations, HyperParameters.MutationRate);
                Mutation(child2, gen, HyperParameters.Generations, HyperParameters.MutationRate);

                child1.Fitness = EvaluateChromosome(child1, AvailableBoxes, out var child1Boxes, gen, HyperParameters.Generations);
                child1.BoxPlacements = child1Boxes;
                child2.Fitness = EvaluateChromosome(child2, AvailableBoxes, out var child2Boxes, gen, HyperParameters.Generations);
                child2.BoxPlacements = child2Boxes;

                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }

            Population = newPopulation.ToList();
        }

        return Population.OrderBy(e => e.Fitness).First();
    }
}
