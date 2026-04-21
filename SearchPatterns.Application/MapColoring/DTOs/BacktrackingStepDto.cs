using SearchPatterns.Domain.MapColoring.Entities;

namespace SearchPatterns.Application.MapColoring.DTOs;

/// <summary>
/// Data Transfer Object for a single backtracking algorithm step.
/// </summary>
public record BacktrackingStepDto
{
    /// <summary>
    /// Sequential step number (0-indexed).
    /// </summary>
    public required int StepNumber { get; init; }

    /// <summary>
    /// The type of action performed in this step.
    /// Values: "TryAssign", "Backtrack", "Success", "Failure"
    /// </summary>
    public required string ActionType { get; init; }

    /// <summary>
    /// The region ID involved in this step.
    /// </summary>
    public required int RegionId { get; init; }

    /// <summary>
    /// The color name involved in this step.
    /// Values: "Red", "Blue", "Green", "Yellow"
    /// </summary>
    public required string ColorName { get; init; }

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
    /// Dictionary key is region ID, value is color name.
    /// </summary>
    public required Dictionary<int, string> CurrentAssignment { get; init; }

    /// <summary>
    /// Timestamp offset in milliseconds from the start of algorithm execution.
    /// </summary>
    public required long TimestampOffsetMs { get; init; }

    /// <summary>
    /// Creates a BacktrackingStepDto from a BacktrackingStep entity.
    /// </summary>
    public static BacktrackingStepDto FromEntity(BacktrackingStep step)
    {
        // Convert ActionType enum to string
        string actionType = step.ActionType.ToString();

        // Convert color index to color name
        string colorName = step.ColorValue switch
        {
            0 => "Red",
            1 => "Blue",
            2 => "Green",
            3 => "Yellow",
            _ => "Unknown"
        };

        // Convert CurrentAssignment Dictionary<int, int> to Dictionary<int, string>
        var currentAssignment = new Dictionary<int, string>();
        foreach (var kvp in step.CurrentAssignment)
        {
            string assignedColorName = kvp.Value switch
            {
                0 => "Red",
                1 => "Blue",
                2 => "Green",
                3 => "Yellow",
                _ => "Unknown"
            };
            currentAssignment[kvp.Key] = assignedColorName;
        }

        return new BacktrackingStepDto
        {
            StepNumber = step.StepNumber,
            ActionType = actionType,
            RegionId = step.RegionId,
            ColorName = colorName,
            IsSafe = step.IsSafe,
            ConflictingRegion = step.ConflictingRegion,
            CurrentAssignment = currentAssignment,
            TimestampOffsetMs = step.TimestampOffsetMs
        };
    }
}
