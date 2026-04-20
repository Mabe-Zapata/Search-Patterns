namespace SearchPatterns.Application.FarmerPuzzle.DTOs;

public record MoveStepDto(
	int StepNumber,
	string Action,
	string Description,
	string FarmerSide,
	string WolfSide,
	string GoatSide,
	string CabbageSide
);