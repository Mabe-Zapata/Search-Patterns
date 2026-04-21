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
    /// <param name="graph">The graph to solve.</param>
    /// <param name="captureSteps">If true, captures step-by-step execution trace.</param>
    public SolutionResult Solve(Graph graph, bool captureSteps = false)
    {
        var assignment = new ColorAssignment();
        var backtrackingSteps = 0;
        var stopwatch = Stopwatch.StartNew();
        var timeoutMs = TimeoutSeconds * 1000;

        // Create step recorder if capture is enabled
        StepRecorder? recorder = captureSteps ? new StepRecorder() : null;

        var result = Backtrack(graph, assignment, 0, ref backtrackingSteps, stopwatch, timeoutMs, recorder);

        stopwatch.Stop();

        return result switch
        {
            BacktrackResult.Success => SolutionResult.Solvable(
                assignment,
                graph.RegionCount,
                backtrackingSteps,
                stopwatch.ElapsedMilliseconds,
                recorder?.Steps),
            
            BacktrackResult.Timeout => SolutionResult.Timeout(
                graph.RegionCount,
                backtrackingSteps,
                stopwatch.ElapsedMilliseconds,
                recorder?.Steps),
            
            _ => SolutionResult.Unsolvable(
                graph.RegionCount,
                backtrackingSteps,
                stopwatch.ElapsedMilliseconds,
                recorder?.Steps)
        };
    }

    private BacktrackResult Backtrack(
        Graph graph,
        ColorAssignment assignment,
        int currentRegion,
        ref int backtrackingSteps,
        Stopwatch stopwatch,
        long timeoutMs,
        StepRecorder? recorder = null)
    {
        // Check timeout at each recursion step
        if (stopwatch.ElapsedMilliseconds > timeoutMs)
        {
            return BacktrackResult.Timeout;
        }

        // Base case: all regions colored
        if (currentRegion == graph.RegionCount)
        {
            recorder?.RecordSuccess(assignment);
            return BacktrackResult.Success;
        }

        // Try each color (0, 1, 2, 3)
        for (int color = 0; color < MaxColors; color++)
        {
            backtrackingSteps++;

            // Check if color is safe (no adjacent region has this color)
            if (IsSafe(graph, assignment, currentRegion, color))
            {
                // Record safe assignment
                recorder?.RecordTryAssign(currentRegion, color, true, null, assignment);

                // Assign color
                assignment.AssignColor(currentRegion, color);

                // Recurse to next region
                var result = Backtrack(graph, assignment, currentRegion + 1, ref backtrackingSteps, stopwatch, timeoutMs, recorder);

                if (result == BacktrackResult.Success)
                {
                    return BacktrackResult.Success;
                }

                if (result == BacktrackResult.Timeout)
                {
                    return BacktrackResult.Timeout;
                }

                // Record backtrack before unassigning
                recorder?.RecordBacktrack(currentRegion, color, assignment);

                // Backtrack: remove color assignment
                assignment.UnassignColor(currentRegion);
            }
            else
            {
                // Record unsafe assignment with conflict
                int? conflictingRegion = FindConflictingRegion(graph, assignment, currentRegion, color);
                recorder?.RecordTryAssign(currentRegion, color, false, conflictingRegion, assignment);
            }
        }

        // No valid color found for this region
        if (currentRegion == 0)
        {
            // Only record failure at the root level (no solution exists)
            recorder?.RecordFailure();
        }
        
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

    /// <summary>
    /// Finds the first adjacent region that has the same color as the one being tested.
    /// Used for step recording to identify conflict sources.
    /// </summary>
    private int? FindConflictingRegion(Graph graph, ColorAssignment assignment, int region, int color)
    {
        foreach (var adjacentRegion in graph.GetAdjacentRegions(region))
        {
            if (assignment.IsAssigned(adjacentRegion) && assignment.GetColor(adjacentRegion) == color)
            {
                return adjacentRegion;
            }
        }

        return null;
    }

    private enum BacktrackResult
    {
        Success,
        Failure,
        Timeout
    }
}
