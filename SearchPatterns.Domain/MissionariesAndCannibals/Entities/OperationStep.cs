using SearchPatterns.Domain.MissionariesAndCannibals.Enums;

namespace SearchPatterns.Domain.MissionariesAndCannibals.Entities;

/// <summary>
/// Represents a single step in the solution, analogous to MoveStep in FarmerPuzzle.
/// </summary>
public record OperationStep(
	int StepNumber,
	OperationType Operation,
	string Action,
	string Description,
	int MissionariesLeft,
	int CannibalsLeft,
	int MissionariesRight,
	int CannibalsRight,
	BankLocation BoatPosition
);
