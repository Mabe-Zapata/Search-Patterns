using SearchPatterns.Domain.FarmerPuzzle.Entities;

namespace SearchPatterns.Domain.FarmerPuzzle.Interfaces;

public interface IFarmerBfsSolver
{
	SolutionResult Solve();
}