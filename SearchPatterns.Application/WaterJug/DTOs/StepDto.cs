namespace SearchPatterns.Application.WaterJug.DTOs;

/// <summary>
/// Data Transfer Object representing a single step in the water jug solution.
/// </summary>
public record StepDto
{
    /// <summary>
    /// The step number in the sequence (starting from 1).
    /// </summary>
    public required int StepNumber { get; init; }

    /// <summary>
    /// The operation performed in this step.
    /// </summary>
    public required string Operation { get; init; }

    /// <summary>
    /// A human-readable description of the step.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The amount of water in jug A after this step (in liters).
    /// </summary>
    public required int JugAAmount { get; init; }

    /// <summary>
    /// The amount of water in jug B after this step (in liters).
    /// </summary>
    public required int JugBAmount { get; init; }
}