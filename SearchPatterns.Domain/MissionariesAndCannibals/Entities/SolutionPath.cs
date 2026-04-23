namespace SearchPatterns.Domain.MissionariesAndCannibals.Entities;

/// <summary>
/// Wraps the overall result of the BFS solver, following the SolutionResult pattern from FarmerPuzzle.
/// </summary>
public record SolutionPath(
	bool IsSolvable,
	List<OperationStep> Steps
)
{
	public int TotalSteps => Steps.Count;

	public static SolutionPath Solved(List<OperationStep> steps)
		=> new(true, steps);

	public static SolutionPath Unsolvable()
		=> new(false, []);
}
