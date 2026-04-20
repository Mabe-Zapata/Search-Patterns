namespace SearchPatterns.Application.MapColoring.DTOs;

/// <summary>
/// Data Transfer Object for solving metadata.
/// </summary>
public record SolvingMetadataDto
{
    /// <summary>
    /// The total number of backtracking steps performed during solving.
    /// </summary>
    public required int BacktrackingSteps { get; init; }

    /// <summary>
    /// The execution time in milliseconds.
    /// </summary>
    public required long ExecutionTimeMs { get; init; }
}