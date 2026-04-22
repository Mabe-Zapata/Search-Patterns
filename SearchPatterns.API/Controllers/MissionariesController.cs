using Microsoft.AspNetCore.Mvc;
using SearchPatterns.Application.MissionariesAndCannibals.DTOs;
using SearchPatterns.Domain.MissionariesAndCannibals.Interfaces;

namespace SearchPatterns.API.Controllers;

[ApiController]
[Route("api/missionaries")]
public class MissionariesController : ControllerBase
{
	private readonly IMissionariesBfsSolver _solver;

	public MissionariesController(IMissionariesBfsSolver solver)
	{
		_solver = solver ?? throw new ArgumentNullException(nameof(solver));
	}

	[HttpGet("solve")]
	[ProducesResponseType(typeof(SolveMissionariesResponseDto), StatusCodes.Status200OK)]
	public IActionResult Solve()
	{
		var result = _solver.Solve();

		var stepDtos = result.Steps
			.Select(s => new MoveStepDto(
				s.StepNumber,
				s.Operation.ToString(),
				s.Action,
				s.Description,
				s.MissionariesLeft,
				s.CannibalsLeft,
				s.MissionariesRight,
				s.CannibalsRight,
				s.BoatPosition.ToString()
			))
			.ToList();

		var response = new SolveMissionariesResponseDto(
			result.IsSolvable,
			result.TotalSteps,
			stepDtos,
			result.IsSolvable ? null : "No se encontró solución."
		);

		return Ok(response);
	}
}
