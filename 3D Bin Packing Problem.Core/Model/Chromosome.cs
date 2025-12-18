using _3D_Bin_Packing_Problem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Bin_Packing_Problem.Core.Model;

/// <summary>
/// Represents a candidate solution composed of gene sequences with associated fitness results.
/// </summary>
public class Chromosome
{
    public Guid Id { get; } = Guid.NewGuid();
    private double _fitness;
    private PackingResultsViewModel? _packingResults;
    private readonly List<GeneSequence> _sequences = [];
    public IReadOnlyList<GeneSequence> Sequences => _sequences;
    public Chromosome(List<GeneSequence> geneSequences)
    {
        // clone to avoid shared reference when creating population
        _sequences = geneSequences.Select(gs => gs.Clone()).ToList();
    }

    public IEnumerator<GeneSequence> GetEnumerator() => Sequences.GetEnumerator();

    public int Count => Sequences.Count;
    public GeneSequence this[int index]
    {
        get => _sequences[index];
        set => _sequences[index] = value;
    }

    public double Fitness => _fitness;
    public PackingResultsViewModel? PackingResults => _packingResults;

    public void SetFitness(FitnessResultViewModel fitnessResultViewModel)
    {
        _fitness = fitnessResultViewModel.Fitness;
        _packingResults = fitnessResultViewModel.PackingResults;
    }

    /// <summary>
    /// Deep clone — completely isolates BinType, GeneSequence, and Genes
    /// </summary>
    public Chromosome Clone()
    {
        var clonedSequences = Sequences.Select(gs => gs.Clone()).ToList();
        var clone = new Chromosome(clonedSequences);
        clone._fitness = _fitness;
        clone._packingResults = null;
        return clone;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Chromosome chromosome => Id.Equals(chromosome.Id),
            null => false,
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString() => $"Count: {Count}, Fitness: {_fitness}";
}
