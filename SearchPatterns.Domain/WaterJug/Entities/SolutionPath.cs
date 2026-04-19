namespace SearchPatterns.Domain.WaterJug.Entities;

/// <summary>
/// Represents a complete solution path for the water jug problem,
/// including the initial state, steps taken, goal state, and solvability status.
/// </summary>
public record SolutionPath(
    WaterState InitialState,
    List<OperationStep> Steps,
    WaterState GoalState,
    bool IsSolvable
)
{
    public int TotalSteps => Steps.Count;

    public static SolutionPath Unsolvable(WaterState initialState)
    {
        return new SolutionPath(
            initialState,
            new List<OperationStep>(),
            initialState,
            false
        );
    }

    public static SolutionPath Solvable(WaterState initialState, List<OperationStep> steps, WaterState goalState)
    {
        return new SolutionPath(
            initialState,
            steps,
            goalState,
            true
        );
    }
}