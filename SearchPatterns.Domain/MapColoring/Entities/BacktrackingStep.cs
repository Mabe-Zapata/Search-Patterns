using SearchPatterns.Domain.MapColoring.Enums;

namespace SearchPatterns.Domain.MapColoring.Entities;

/// <summary>
/// Represents a single step in the backtracking algorithm execution.
/// Captures the action taken, the region and color involved, and a complete snapshot of the assignment state.
/// </summary>
public sealed class BacktrackingStep
{
    /// <summary>
    /// Sequential step number (0-indexed).
    /// </summary>
    public required int StepNumber { get; init; }

    /// <summary>
    /// The type of action performed in this step.
    /// </summary>
    public required BacktrackingActionType ActionType { get; init; }

    /// <summary>
    /// The region ID involved in this step.
    /// </summary>
    public required int RegionId { get; init; }

    /// <summary>
    /// The color value involved in this step (0-3 for Red, Blue, Green, Yellow).
    /// </summary>
    public required int ColorValue { get; init; }

    /// <summary>
    /// Indicates whether the color assignment was safe (no conflicts with adjacent regions).
    /// </summary>
    public required bool IsSafe { get; init; }

    /// <summary>
    /// The ID of the conflicting region if IsSafe is false, otherwise null.
    /// </summary>
    public int? ConflictingRegion { get; init; }

    /// <summary>
    /// Complete snapshot of the color assignment state at this step.
    /// Maps region ID to color value.
    /// </summary>
    public required Dictionary<int, int> CurrentAssignment { get; init; }

    /// <summary>
    /// Timestamp offset in milliseconds from the start of algorithm execution.
    /// </summary>
    public required long TimestampOffsetMs { get; init; }
}
