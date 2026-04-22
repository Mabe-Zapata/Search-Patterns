using SearchPatterns.Domain.CityRouting.Entities;
using SearchPatterns.Domain.CityRouting.Interfaces;

namespace SearchPatterns.Application.CityRouting.Strategies;

/// <summary>
/// Implementación concreta del Patrón Estrategia usando el algoritmo de Dijkstra.
/// Explora los caminos basándose únicamente en el costo acumulado desde el origen.
/// Garantiza la ruta más corta, pero explora en todas direcciones como una ola de agua.
/// </summary>
public class DijkstraStrategy : IShortestPathStrategy
{
    public PathResult FindShortestPath(CityGraph graph, CityNode startNode, CityNode targetNode)
    {
        // 1. Inicialización
        var gScore = new Dictionary<string, double>();
        var cameFrom = new Dictionary<string, string>();
        
        // C# Priority Queue (NodeName, Cost)
        var openSet = new PriorityQueue<string, double>();

        foreach (var node in graph.Nodes)
        {
            gScore[node.Name] = double.PositiveInfinity;
        }

        gScore[startNode.Name] = 0;
        openSet.Enqueue(startNode.Name, 0);

        int nodesEvaluated = 0;

        // 2. Ciclo principal
        while (openSet.Count > 0)
        {
            var currentName = openSet.Dequeue();
            nodesEvaluated++;

            if (currentName == targetNode.Name)
            {
                var (path, pathString) = ReconstructPath(cameFrom, currentName);
                return PathResult.Success(
                    pathString, 
                    gScore[targetNode.Name]
                );
            }

            var neighbors = graph.GetNeighbors(currentName);
            foreach (var edge in neighbors)
            {
                var neighborName = edge.Destination.Name;
                double tentativeGScore = gScore[currentName] + edge.DistanceKm;

                // Si encontramos un camino más corto hacia este vecino
                if (tentativeGScore < gScore[neighborName])
                {
                    cameFrom[neighborName] = currentName;
                    gScore[neighborName] = tentativeGScore;
                    
                    // Aseguramos reenfilar con la menor prioridad
                    openSet.Enqueue(neighborName, tentativeGScore);
                }
            }
        }

        return PathResult.Failure("No se encontró una ruta hacia el destino con Dijkstra.");
    }

    private (List<string> Nodes, List<string> Description) ReconstructPath(Dictionary<string, string> cameFrom, string current)
    {
        var path = new List<string> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        var descriptionTokens = new List<string>();
        for (int i = 0; i < path.Count; i++)
        {
            descriptionTokens.Add(path[i]);
            if (i < path.Count - 1)
                descriptionTokens.Add("->");
        }

        return (path, descriptionTokens);
    }
}
