using SearchPatterns.Domain.MapColoring.Entities;

namespace SearchPatterns.Domain.MapColoring.Interfaces;

/// <summary>
/// Interface for validating graph inputs.
/// </summary>
public interface IGraphValidator
{
    /// <summary>
    /// Validates a graph structure.
    /// </summary>
    /// <param name="regionCount">Number of regions in the graph.</param>
    /// <param name="adjacencyList">Adjacency list representation (optional).</param>
    /// <param name="adjacencyMatrix">Adjacency matrix representation as jagged array (optional).</param>
    /// <returns>A validation result containing errors if validation fails.</returns>
    ValidationResult Validate(
        int regionCount, 
        Dictionary<int, List<int>>? adjacencyList, 
        int[][]? adjacencyMatrix
    );
}
