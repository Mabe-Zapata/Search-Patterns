using SearchPatterns.Domain.MapColoring.Entities;

namespace SearchPatterns.Domain.MapColoring.Interfaces;

/// <summary>
/// Interface for the backtracking solver that solves the map coloring problem.
/// </summary>
public interface IBacktrackingSolver
{
    /// <summary>
    /// Solves the map coloring problem using backtracking algorithm.
    /// </summary>
    /// <param name="graph">The graph representing the map.</param>
    /// <returns>A SolutionResult containing the color assignment or failure information.</returns>
    SolutionResult Solve(Graph graph);
}
