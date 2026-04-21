using SearchPatterns.Domain.MapColoring.Entities;
using SearchPatterns.Domain.MapColoring.Enums;

namespace SearchPatterns.Application.MapColoring.DTOs;

/// <summary>
/// Data Transfer Object for the map coloring solution response.
/// </summary>
public record SolveResponseDto
{
    /// <summary>
    /// Indicates whether a valid coloring was found.
    /// </summary>
    public required bool IsSolvable { get; init; }

    /// <summary>
    /// The color assignment for each region. Dictionary key is region ID, value is color name.
    /// </summary>
    public Dictionary<int, string>? ColorAssignment { get; init; }

    /// <summary>
    /// The total number of regions.
    /// </summary>
    public required int TotalRegions { get; init; }

    /// <summary>
    /// Metadata about the solving process.
    /// </summary>
    public SolvingMetadataDto? Metadata { get; init; }

    /// <summary>
    /// Error message if the solution is not possible or invalid input was provided.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Optional step-by-step execution trace. Null when captureSteps was false in the request.
    /// </summary>
    public List<BacktrackingStepDto>? Steps { get; init; }

    /// <summary>
    /// Creates a SolveResponseDto from a SolutionResult entity.
    /// </summary>
    public static SolveResponseDto FromEntity(SolutionResult result)
    {
        Dictionary<int, string>? colorAssignment = null;

        if (result.IsSolvable && result.ColorAssignment != null)
        {
            colorAssignment = new Dictionary<int, string>();
            for (int region = 0; region < result.TotalRegions; region++)
            {
                int colorIndex = result.ColorAssignment.GetColor(region);
                if (colorIndex >= 0 && colorIndex <= 3)
                {
                    string colorName = colorIndex switch
                    {
                        0 => "Red",
                        1 => "Blue",
                        2 => "Green",
                        3 => "Yellow",
                        _ => "Unknown"
                    };
                    colorAssignment[region] = colorName;
                }
            }
        }

        // Map steps if available
        List<BacktrackingStepDto>? steps = null;
        if (result.Steps != null)
        {
            steps = result.Steps.Select(BacktrackingStepDto.FromEntity).ToList();
        }

        return new SolveResponseDto
        {
            IsSolvable = result.IsSolvable,
            ColorAssignment = colorAssignment,
            TotalRegions = result.TotalRegions,
            Metadata = new SolvingMetadataDto
            {
                BacktrackingSteps = result.BacktrackingSteps,
                ExecutionTimeMs = result.ExecutionTimeMs
            },
            ErrorMessage = null,
            Steps = steps
        };
    }

    /// <summary>
    /// Creates an error response.
    /// </summary>
    public static SolveResponseDto Error(string errorMessage)
    {
        return new SolveResponseDto
        {
            IsSolvable = false,
            ColorAssignment = null,
            TotalRegions = 0,
            Metadata = null,
            ErrorMessage = errorMessage
        };
    }

    /// <summary>
    /// Creates an unsolvable response.
    /// </summary>
    public static SolveResponseDto Unsolvable(SolutionResult result, string errorMessage)
    {
        // Map steps if available
        List<BacktrackingStepDto>? steps = null;
        if (result.Steps != null)
        {
            steps = result.Steps.Select(BacktrackingStepDto.FromEntity).ToList();
        }

        return new SolveResponseDto
        {
            IsSolvable = false,
            ColorAssignment = null,
            TotalRegions = result.TotalRegions,
            Metadata = new SolvingMetadataDto
            {
                BacktrackingSteps = result.BacktrackingSteps,
                ExecutionTimeMs = result.ExecutionTimeMs
            },
            ErrorMessage = errorMessage,
            Steps = steps
        };
    }
}