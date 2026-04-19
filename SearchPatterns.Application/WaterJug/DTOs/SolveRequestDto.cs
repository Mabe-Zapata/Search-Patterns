namespace SearchPatterns.Application.WaterJug.DTOs;

/// <summary>
/// Data Transfer Object for requesting a water jug solution.
/// </summary>
public record SolveRequestDto
{
    /// <summary>
    /// The capacity of jug A in liters.
    /// </summary>
    public required int JugACapacity { get; init; }

    /// <summary>
    /// The capacity of jug B in liters.
    /// </summary>
    public required int JugBCapacity { get; init; }

    /// <summary>
    /// The target amount of water to measure in liters.
    /// </summary>
    public required int TargetAmount { get; init; }
}