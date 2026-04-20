namespace SearchPatterns.Domain.FarmerPuzzle.Entities;

public record SolutionResult(
	bool IsSolvable,
	List<MoveStep> Steps
)
{
	public int TotalSteps => Steps.Count;

	public static SolutionResult Solved(List<MoveStep> steps)
		=> new(true, steps);

	public static SolutionResult Unsolvable()
		=> new(false, []);
}