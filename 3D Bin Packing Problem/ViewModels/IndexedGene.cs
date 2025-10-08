using _3D_Bin_Packing_Problem.Model;

namespace _3D_Bin_Packing_Problem.ViewModels;

/// <summary>
/// Provides the gene metadata required for view model projections, including sequence indices and positions.
/// </summary>
public record IndexedGene(Gene Gene, int SequenceIndex, int PositionInSequence);
