using Microsoft.AspNetCore.Mvc;
using SearchPatterns.Application.FarmerPuzzle.DTOs;
using SearchPatterns.Domain.FarmerPuzzle.Interfaces;

namespace SearchPatterns.API.Controllers;

[ApiController]
[Route("api/farmerpuzzle")]
public class FarmerPuzzleController : ControllerBase
{
	private readonly IFarmerBfsSolver _solver;

	public FarmerPuzzleController(IFarmerBfsSolver solver)
	{
		_solver = solver ?? throw new ArgumentNullException(nameof(solver));
	}

	[HttpGet("solve")]
	[ProducesResponseType(typeof(SolveFarmerResponseDto), StatusCodes.Status200OK)]
	public IActionResult Solve()
	{
		var result = _solver.Solve();

		var stepDtos = result.Steps
			.Select(s => new MoveStepDto(
				s.StepNumber,
				s.Action,
				s.Description,
				s.FarmerSide.ToString(),
				s.WolfSide.ToString(),
				s.GoatSide.ToString(),
				s.CabbageSide.ToString()
			))
			.ToList();

		var response = new SolveFarmerResponseDto(
			result.IsSolvable,
			result.TotalSteps,
			stepDtos,
			result.IsSolvable ? null : "No se encontró solución."
		);

		return Ok(response);
	}
}