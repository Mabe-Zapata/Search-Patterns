using SearchPatterns.Domain.WaterJug.Enums;

namespace SearchPatterns.Domain.WaterJug.Entities;

/// <summary>
/// Represents a single step in the solution path, containing the operation performed,
/// its description, and the resulting water state.
/// </summary>
public record OperationStep(
    OperationType Operation,
    string Description,
    WaterState State
);