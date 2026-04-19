using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SearchPatterns.Application.WaterJug.DTOs;
using SearchPatterns.Application.WaterJug.Validators;
using SearchPatterns.Domain.WaterJug.Entities;
using SearchPatterns.Domain.WaterJug.Interfaces;

namespace SearchPatterns.API.Controllers;

/// <summary>
/// API controller for the Water Jug problem solver.
/// </summary>
[ApiController]
[Route("api/waterjug")]
public class WaterJugController : ControllerBase
{
    private readonly IWaterJugValidator _validator;
    private readonly IBfsSolver _solver;

    /// <summary>
    /// Initializes a new instance of the <see cref="WaterJugController"/> class.
    /// </summary>
    /// <param name="validator">The water jug validator.</param>
    /// <param name="solver">The BFS solver for the water jug problem.</param>
    public WaterJugController(IWaterJugValidator validator, IBfsSolver solver)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _solver = solver ?? throw new ArgumentNullException(nameof(solver));
    }

    /// <summary>
    /// Solves the water jug problem using BFS algorithm.
    /// </summary>
    /// <param name="request">The solve request containing jug capacities and target amount.</param>
    /// <returns>A response containing the solution steps or an error message.</returns>
    /// <response code="200">Returns the solution to the water jug problem.</response>
    /// <response code="400">Returns validation errors if the input is invalid.</response>
    [HttpPost("solve")]
    [ProducesResponseType(typeof(SolveResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Solve([FromBody] SolveRequestDto request)
    {
        // Validate the request
        var validationResult = _validator.Validate(
            request.JugACapacity,
            request.JugBCapacity,
            request.TargetAmount
        );

        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "One or more validation errors occurred",
                status = 400,
                errors = validationResult.Errors
            });
        }

        // Solve the problem
        var solution = _solver.Solve(
            request.JugACapacity,
            request.JugBCapacity,
            request.TargetAmount
        );

        // Map the solution to response DTO
        var response = MapToResponse(solution);
        return Ok(response);
    }

    /// <summary>
    /// Maps a SolutionPath to a SolveResponseDto.
    /// </summary>
    /// <param name="solution">The solution path from the solver.</param>
    /// <returns>A SolveResponseDto containing the solution.</returns>
    private static SolveResponseDto MapToResponse(SolutionPath solution)
    {
        var steps = solution.Steps.Select((step, index) => new StepDto
        {
            StepNumber = index + 1,
            Operation = step.Operation.ToString(),
            Description = step.Description,
            JugAAmount = step.State.JugACurrent,
            JugBAmount = step.State.JugBCurrent
        }).ToList();

        return new SolveResponseDto
        {
            IsSolvable = solution.IsSolvable,
            TotalSteps = solution.TotalSteps,
            Steps = steps,
            ErrorMessage = solution.IsSolvable ? null : "No existe solución para los parámetros dados"
        };
    }
}
