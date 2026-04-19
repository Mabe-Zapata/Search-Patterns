namespace SearchPatterns.Application.WaterJug.DTOs;

/// <summary>
/// Data Transfer Object for the water jug solution response.
/// </summary>
public record SolveResponseDto
{
    /// <summary>
    /// Indicates whether the target amount is achievable with the given jug capacities.
    /// </summary>
    public required bool IsSolvable { get; init; }

    /// <summary>
    /// The total number of steps required to reach the solution.
    /// </summary>
    public required int TotalSteps { get; init; }

    /// <summary>
    /// The list of steps taken to solve the problem.
    /// </summary>
    public required IReadOnlyList<StepDto> Steps { get; init; }

    /// <summary>
    /// Error message if the solution is not possible or invalid input was provided.
    /// </summary>
    public string? ErrorMessage { get; init; }
}