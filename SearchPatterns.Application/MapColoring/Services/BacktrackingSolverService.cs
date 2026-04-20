using System.Diagnostics;
using SearchPatterns.Domain.MapColoring.Entities;
using SearchPatterns.Domain.MapColoring.Interfaces;

namespace SearchPatterns.Application.MapColoring.Services;

/// <summary>
/// Service that solves the map coloring problem using backtracking algorithm.
/// </summary>
public class BacktrackingSolverService : IBacktrackingSolver
{
    private const int TimeoutSeconds = 30;
    private const int MaxColors = 4;

    /// <summary>
    /// Solves the map coloring problem using backtracking algorithm.
    /// </summary>
    public SolutionResult Solve(Graph graph)
    {
        var assignment = new ColorAssignment();
        var backtrackingSteps = 0;
        var stopwatch = Stopwatch.StartNew();
        var timeoutMs = TimeoutSeconds * 1000;

        var result = Backtrack(graph, assignment, 0, ref backtrackingSteps, stopwatch, timeoutMs);

        stopwatch.Stop();

        return result switch
        {
            BacktrackResult.Success => SolutionResult.Solvable(
                assignment,
                graph.RegionCount,
                backtrackingSteps,
                stopwatch.ElapsedMilliseconds),
            
            BacktrackResult.Timeout => SolutionResult.Timeout(
                graph.RegionCount,
                backtrackingSteps,
                stopwatch.ElapsedMilliseconds),
            
            _ => SolutionResult.Unsolvable(
                graph.RegionCount,
                backtrackingSteps,
                stopwatch.ElapsedMilliseconds)
        };
    }

    private BacktrackResult Backtrack(
        Graph graph,
        ColorAssignment assignment,
        int currentRegion,
        ref int backtrackingSteps,
        Stopwatch stopwatch,
        long timeoutMs)
    {
        // Check timeout at each recursion step
        if (stopwatch.ElapsedMilliseconds > timeoutMs)
        {
            return BacktrackResult.Timeout;
        }

        // Base case: all regions colored
        if (currentRegion == graph.RegionCount)
        {
            return BacktrackResult.Success;
        }

        // Try each color (0, 1, 2, 3)
        for (int color = 0; color < MaxColors; color++)
        {
            backtrackingSteps++;

            // Check if color is safe (no adjacent region has this color)
            if (IsSafe(graph, assignment, currentRegion, color))
            {
                // Assign color
                assignment.AssignColor(currentRegion, color);

                // Recurse to next region
                var result = Backtrack(graph, assignment, currentRegion + 1, ref backtrackingSteps, stopwatch, timeoutMs);

                if (result == BacktrackResult.Success)
                {
                    return BacktrackResult.Success;
                }

                if (result == BacktrackResult.Timeout)
                {
                    return BacktrackResult.Timeout;
                }

                // Backtrack: remove color assignment
                assignment.UnassignColor(currentRegion);
            }
        }

        // No valid color found for this region
        return BacktrackResult.Failure;
    }

    private bool IsSafe(Graph graph, ColorAssignment assignment, int region, int color)
    {
        // Check all adjacent regions for color conflicts
        foreach (var adjacentRegion in graph.GetAdjacentRegions(region))
        {
            // If adjacent region is assigned and has the same color, conflict!
            if (assignment.IsAssigned(adjacentRegion) && assignment.GetColor(adjacentRegion) == color)
            {
                return false;
            }
        }

        // No conflicts, color is safe to assign
        return true;
    }

    private enum BacktrackResult
    {
        Success,
        Failure,
        Timeout
    }
}
