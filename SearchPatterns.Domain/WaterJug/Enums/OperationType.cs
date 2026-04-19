namespace SearchPatterns.Domain.WaterJug.Enums;

/// <summary>
/// Represents the type of operation that can be performed on the water jugs.
/// </summary>
public enum OperationType
{
    /// <summary>Fill Jug A to its maximum capacity.</summary>
    FillA,
    
    /// <summary>Fill Jug B to its maximum capacity.</summary>
    FillB,
    
    /// <summary>Empty Jug A (set to 0 liters).</summary>
    EmptyA,
    
    /// <summary>Empty Jug B (set to 0 liters).</summary>
    EmptyB,
    
    /// <summary>Transfer water from Jug A to Jug B.</summary>
    TransferAtoB,
    
    /// <summary>Transfer water from Jug B to Jug A.</summary>
    TransferBtoA
}