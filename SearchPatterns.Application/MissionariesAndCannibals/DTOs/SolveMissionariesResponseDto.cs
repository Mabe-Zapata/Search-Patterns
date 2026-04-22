namespace SearchPatterns.Application.MissionariesAndCannibals.DTOs;

public record SolveMissionariesResponseDto(
	bool IsSolvable,
	int TotalSteps,
	List<MoveStepDto> Steps,
	string? ErrorMessage
);
