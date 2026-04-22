namespace SearchPatterns.Application.MissionariesAndCannibals.DTOs;

public record MoveStepDto(
	int StepNumber,
	string Operation,
	string Action,
	string Description,
	int MissionariesLeft,
	int CannibalsLeft,
	int MissionariesRight,
	int CannibalsRight,
	string BoatPosition
);
