namespace SearchPatterns.Application.MapColoring.DTOs;

/// <summary>
/// Data Transfer Object for requesting a map coloring solution.
/// </summary>
public record SolveRequestDto
{
    /// <summary>
    /// The number of regions in the map.
    /// </summary>
    public required int RegionCount { get; init; }

    /// <summary>
    /// Adjacency list representation of the graph (optional).
    /// Dictionary where key is region ID and value is list of adjacent region IDs.
    /// </summary>
    public Dictionary<int, List<int>>? AdjacencyList { get; init; }

    /// <summary>
    /// Adjacency matrix representation of the graph (optional).
    /// 2D array where matrix[i][j] = 1 means regions i and j are adjacent, 0 otherwise.
    /// </summary>
    public int[][]? AdjacencyMatrix { get; init; }

    /// <summary>
    /// If true, the solver will capture step-by-step execution trace for visualization.
    /// Defaults to false for backward compatibility.
    /// </summary>
    public bool CaptureSteps { get; init; } = false;
}