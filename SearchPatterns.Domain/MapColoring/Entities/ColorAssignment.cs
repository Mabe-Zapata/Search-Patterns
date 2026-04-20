namespace SearchPatterns.Domain.MapColoring.Entities;

/// <summary>
/// Represents a color assignment for all regions in the graph.
/// </summary>
public sealed class ColorAssignment
{
    /// <summary>
    /// Maps region ID to assigned color (0-3).
    /// </summary>
    public Dictionary<int, int> Assignment { get; init; } = new();

    /// <summary>
    /// Gets the color assigned to a specific region.
    /// Returns -1 if the region has not been assigned a color yet.
    /// </summary>
    public int GetColor(int region)
    {
        return Assignment.TryGetValue(region, out var color) ? color : -1;
    }

    /// <summary>
    /// Assigns a color to a region.
    /// </summary>
    public void AssignColor(int region, int color)
    {
        Assignment[region] = color;
    }

    /// <summary>
    /// Removes the color assignment for a region (used during backtracking).
    /// </summary>
    public void UnassignColor(int region)
    {
        Assignment.Remove(region);
    }

    /// <summary>
    /// Checks if a region has been assigned a color.
    /// </summary>
    public bool IsAssigned(int region)
    {
        return Assignment.ContainsKey(region);
    }
}
