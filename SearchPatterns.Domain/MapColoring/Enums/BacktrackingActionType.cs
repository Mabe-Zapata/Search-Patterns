namespace SearchPatterns.Domain.MapColoring.Enums;

/// <summary>
/// Represents the type of action taken during backtracking algorithm execution.
/// </summary>
public enum BacktrackingActionType
{
    /// <summary>
    /// Attempting to assign a color to a region.
    /// </summary>
    TryAssign,

    /// <summary>
    /// Removing a color assignment (backtracking).
    /// </summary>
    Backtrack,

    /// <summary>
    /// Solution found successfully.
    /// </summary>
    Success,

    /// <summary>
    /// No solution exists for the given graph.
    /// </summary>
    Failure
}
