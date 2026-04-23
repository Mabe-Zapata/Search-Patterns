using SearchPatterns.Domain.CityRouting.Entities;

namespace SearchPatterns.Domain.CityRouting.Interfaces;

/// <summary>
/// El "Strategy" principal del patrón homónimo, que define cómo buscar una ruta.
/// Cualquier algoritmo de navegación deberá implementar este contrato.
/// </summary>
public interface IShortestPathStrategy
{
    /// <summary>
    /// Encuentra la ruta más corta (o la más económica) desde el origen hasta el objetivo en un grafo.
    /// </summary>
    PathResult FindShortestPath(CityGraph graph, CityNode startNode, CityNode targetNode);
}
