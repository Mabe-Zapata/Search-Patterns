namespace SearchPatterns.Domain.MapColoring.Entities;

/// <summary>
/// Represents the result of attempting to solve the map coloring problem.
/// </summary>
public record SolutionResult(
    bool IsSolvable,
    ColorAssignment ColorAssignment,
    int TotalRegions,
    int BacktrackingSteps,
    long ExecutionTimeMs,
    IReadOnlyList<BacktrackingStep>? Steps = null
)
{
    /// <summary>
    /// Creates a solvable result with the color assignment.
    /// </summary>
    public static SolutionResult Solvable(
        ColorAssignment assignment, 
        int totalRegions, 
        int backtrackingSteps, 
        long executionTimeMs,
        IReadOnlyList<BacktrackingStep>? steps = null)
    {
        return new SolutionResult(
            true, 
            assignment, 
            totalRegions, 
            backtrackingSteps, 
            executionTimeMs,
            steps
        );
    }

    /// <summary>
    /// Creates an unsolvable result.
    /// </summary>
    public static SolutionResult Unsolvable(
        int totalRegions, 
        int backtrackingSteps, 
        long executionTimeMs,
        IReadOnlyList<BacktrackingStep>? steps = null)
    {
        return new SolutionResult(
            false, 
            new ColorAssignment(), 
            totalRegions, 
            backtrackingSteps, 
            executionTimeMs,
            steps
        );
    }

    /// <summary>
    /// Creates a timeout result.
    /// </summary>
    public static SolutionResult Timeout(
        int totalRegions, 
        int backtrackingSteps, 
        long executionTimeMs,
        IReadOnlyList<BacktrackingStep>? steps = null)
    {
        return new SolutionResult(
            false, 
            new ColorAssignment(), 
            totalRegions, 
            backtrackingSteps, 
            executionTimeMs,
            steps
        );
    }
}
