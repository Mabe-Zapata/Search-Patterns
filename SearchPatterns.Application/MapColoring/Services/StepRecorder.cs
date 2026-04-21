using System.Diagnostics;
using SearchPatterns.Domain.MapColoring.Entities;
using SearchPatterns.Domain.MapColoring.Enums;

namespace SearchPatterns.Application.MapColoring.Services;

/// <summary>
/// Records step-by-step execution trace of the backtracking algorithm.
/// Captures color assignment attempts, backtracking events, and complete state snapshots.
/// </summary>
public sealed class StepRecorder
{
    private readonly List<BacktrackingStep> _steps = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private int _stepCounter = 0;

    /// <summary>
    /// Gets the read-only collection of recorded steps.
    /// </summary>
    public IReadOnlyList<BacktrackingStep> Steps => _steps.AsReadOnly();

    /// <summary>
    /// Records a color assignment attempt.
    /// </summary>
    /// <param name="region">The region being colored.</param>
    /// <param name="color">The color being assigned.</param>
    /// <param name="isSafe">Whether the assignment is safe (no conflicts).</param>
    /// <param name="conflictingRegion">The ID of the conflicting region if unsafe, otherwise null.</param>
    /// <param name="currentState">The current color assignment state.</param>
    public void RecordTryAssign(
        int region, 
        int color, 
        bool isSafe, 
        int? conflictingRegion, 
        ColorAssignment currentState)
    {
        var step = new BacktrackingStep
        {
            StepNumber = _stepCounter++,
            ActionType = BacktrackingActionType.TryAssign,
            RegionId = region,
            ColorValue = color,
            IsSafe = isSafe,
            ConflictingRegion = conflictingRegion,
            CurrentAssignment = CloneAssignment(currentState),
            TimestampOffsetMs = _stopwatch.ElapsedMilliseconds
        };

        _steps.Add(step);
    }

    /// <summary>
    /// Records a backtracking event (removing a color assignment).
    /// </summary>
    /// <param name="region">The region being uncolored.</param>
    /// <param name="color">The color being removed.</param>
    /// <param name="currentState">The current color assignment state.</param>
    public void RecordBacktrack(int region, int color, ColorAssignment currentState)
    {
        var step = new BacktrackingStep
        {
            StepNumber = _stepCounter++,
            ActionType = BacktrackingActionType.Backtrack,
            RegionId = region,
            ColorValue = color,
            IsSafe = false,
            ConflictingRegion = null,
            CurrentAssignment = CloneAssignment(currentState),
            TimestampOffsetMs = _stopwatch.ElapsedMilliseconds
        };

        _steps.Add(step);
    }

    /// <summary>
    /// Records a successful solution.
    /// </summary>
    /// <param name="finalState">The final color assignment state.</param>
    public void RecordSuccess(ColorAssignment finalState)
    {
        var step = new BacktrackingStep
        {
            StepNumber = _stepCounter++,
            ActionType = BacktrackingActionType.Success,
            RegionId = -1,
            ColorValue = -1,
            IsSafe = true,
            ConflictingRegion = null,
            CurrentAssignment = CloneAssignment(finalState),
            TimestampOffsetMs = _stopwatch.ElapsedMilliseconds
        };

        _steps.Add(step);
    }

    /// <summary>
    /// Records a failure (no solution exists).
    /// </summary>
    public void RecordFailure()
    {
        var step = new BacktrackingStep
        {
            StepNumber = _stepCounter++,
            ActionType = BacktrackingActionType.Failure,
            RegionId = -1,
            ColorValue = -1,
            IsSafe = false,
            ConflictingRegion = null,
            CurrentAssignment = new Dictionary<int, int>(),
            TimestampOffsetMs = _stopwatch.ElapsedMilliseconds
        };

        _steps.Add(step);
    }

    /// <summary>
    /// Creates an independent copy of the color assignment state.
    /// </summary>
    private Dictionary<int, int> CloneAssignment(ColorAssignment assignment)
    {
        var clone = new Dictionary<int, int>();
        
        foreach (var kvp in assignment.Assignment)
        {
            clone[kvp.Key] = kvp.Value;
        }

        return clone;
    }
}
