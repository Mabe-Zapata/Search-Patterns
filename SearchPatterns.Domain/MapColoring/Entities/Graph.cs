namespace SearchPatterns.Domain.MapColoring.Entities;

/// <summary>
/// Represents a graph structure for the map coloring problem.
/// Supports both adjacency list and adjacency matrix representations.
/// </summary>
public sealed class Graph
{
    /// <summary>
    /// Number of regions (vertices) in the graph.
    /// </summary>
    public int RegionCount { get; init; }

    /// <summary>
    /// Adjacency list representation: region ID -> list of adjacent region IDs.
    /// Null if adjacency matrix is used.
    /// </summary>
    public Dictionary<int, List<int>>? AdjacencyList { get; init; }

    /// <summary>
    /// Adjacency matrix representation: [i][j] = true if regions i and j are adjacent.
    /// Null if adjacency list is used.
    /// </summary>
    public bool[,]? AdjacencyMatrix { get; init; }

    /// <summary>
    /// Checks if two regions are adjacent.
    /// </summary>
    public bool AreAdjacent(int regionA, int regionB)
    {
        if (AdjacencyList != null)
        {
            return AdjacencyList.ContainsKey(regionA) && 
                   AdjacencyList[regionA].Contains(regionB);
        }
        
        if (AdjacencyMatrix != null)
        {
            return AdjacencyMatrix[regionA, regionB];
        }
        
        return false;
    }

    /// <summary>
    /// Gets all adjacent regions for a given region.
    /// </summary>
    public IEnumerable<int> GetAdjacentRegions(int region)
    {
        if (AdjacencyList != null && AdjacencyList.ContainsKey(region))
        {
            return AdjacencyList[region];
        }
        
        if (AdjacencyMatrix != null)
        {
            var adjacent = new List<int>();
            for (int i = 0; i < RegionCount; i++)
            {
                if (AdjacencyMatrix[region, i])
                {
                    adjacent.Add(i);
                }
            }
            return adjacent;
        }
        
        return Enumerable.Empty<int>();
    }
}
