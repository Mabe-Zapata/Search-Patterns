using SearchPatterns.Domain.FarmerPuzzle.Enums;

namespace SearchPatterns.Domain.FarmerPuzzle.Entities;

public record MoveStep(
	int StepNumber,
	string Action,
	string Description,
	Side FarmerSide,
	Side WolfSide,
	Side GoatSide,
	Side CabbageSide
);