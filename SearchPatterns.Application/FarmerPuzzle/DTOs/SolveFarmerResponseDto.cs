namespace SearchPatterns.Application.FarmerPuzzle.DTOs;

public record SolveFarmerResponseDto(
	bool IsSolvable,
	int TotalSteps,
	List<MoveStepDto> Steps,
	string? ErrorMessage
);