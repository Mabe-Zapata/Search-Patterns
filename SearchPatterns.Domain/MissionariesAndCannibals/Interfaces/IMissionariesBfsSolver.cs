using SearchPatterns.Domain.MissionariesAndCannibals.Entities;

namespace SearchPatterns.Domain.MissionariesAndCannibals.Interfaces;

/// <summary>
/// Interface for the BFS solver of the Missionaries and Cannibals problem.
/// Follows the Dependency Inversion Principle (DIP) from SOLID.
/// </summary>
public interface IMissionariesBfsSolver
{
	SolutionPath Solve();
}
