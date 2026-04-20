using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SearchPatterns.Application.MapColoring.DTOs;
using SearchPatterns.Domain.MapColoring.Entities;
using SearchPatterns.Domain.MapColoring.Interfaces;

namespace SearchPatterns.API.Controllers;

/// <summary>
/// API controller for the Map Coloring problem solver.
/// </summary>
[ApiController]
[Route("api/mapcoloring")]
public class MapColoringController : ControllerBase
{
    private readonly IGraphValidator _validator;
    private readonly IBacktrackingSolver _solver;
    private readonly ILogger<MapColoringController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapColoringController"/> class.
    /// </summary>
    /// <param name="validator">The graph validator.</param>
    /// <param name="solver">The backtracking solver for the map coloring problem.</param>
    /// <param name="logger">The logger.</param>
    public MapColoringController(
        IGraphValidator validator, 
        IBacktrackingSolver solver,
        ILogger<MapColoringController> logger)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _solver = solver ?? throw new ArgumentNullException(nameof(solver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Solves the map coloring problem using backtracking algorithm.
    /// </summary>
    /// <param name="request">The solve request containing graph data.</param>
    /// <returns>A response containing the color assignment or an error message.</returns>
    /// <response code="200">Returns the solution to the map coloring problem.</response>
    /// <response code="400">Returns validation errors if the input is invalid.</response>
    [HttpPost("solve")]
    [ProducesResponseType(typeof(SolveResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Solve([FromBody] SolveRequestDto request)
    {
        try
        {
            // Validate the request
            var validationResult = _validator.Validate(
                request.RegionCount,
                request.AdjacencyList,
                request.AdjacencyMatrix
            );

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Validation failed: {Errors}", string.Join(", ", validationResult.Errors));
                return BadRequest(new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "One or more validation errors occurred",
                    status = 400,
                    errors = validationResult.Errors
                });
            }

            // Create Graph entity from validated request
            // Convert int[][] to bool[,] if matrix is provided
            bool[,]? boolMatrix = null;
            if (request.AdjacencyMatrix != null)
            {
                boolMatrix = new bool[request.RegionCount, request.RegionCount];
                for (int i = 0; i < request.RegionCount; i++)
                {
                    for (int j = 0; j < request.RegionCount; j++)
                    {
                        boolMatrix[i, j] = request.AdjacencyMatrix[i][j] != 0;
                    }
                }
            }

            var graph = new Graph
            {
                RegionCount = request.RegionCount,
                AdjacencyList = request.AdjacencyList,
                AdjacencyMatrix = boolMatrix
            };

            // Solve the problem
            var solution = _solver.Solve(graph);

            // Map the solution to response DTO
            SolveResponseDto response;

            if (solution.IsSolvable)
            {
                response = SolveResponseDto.FromEntity(solution);
            }
            else
            {
                // Check if it's a timeout or unsolvable
                string errorMessage = "No se encontró una coloración válida para este grafo";
                
                // Check if the solution might have timed out by looking at execution time
                if (solution.ExecutionTimeMs >= 30000)
                {
                    errorMessage = "El tiempo de ejecución excedió el límite de 30 segundos";
                }

                response = SolveResponseDto.Unsolvable(solution, errorMessage);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while solving map coloring problem");
            return StatusCode(500, new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "An unexpected error occurred",
                status = 500,
                detail = "Ocurrió un error inesperado al procesar la solicitud"
            });
        }
    }
}