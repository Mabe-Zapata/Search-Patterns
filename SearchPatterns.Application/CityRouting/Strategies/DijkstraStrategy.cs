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
        var gScore = new Dictionary<string, double>();
        var cameFrom = new Dictionary<string, string>();
        var explorationSteps = new List<ExplorationStep>();
        
        var openSet = new PriorityQueue<string, double>();

        foreach (var node in graph.Nodes)
        {
            gScore[node.Name] = double.PositiveInfinity;
        }

        gScore[startNode.Name] = 0;
        openSet.Enqueue(startNode.Name, 0);

        int stepCount = 0;

        explorationSteps.Add(new ExplorationStep
        {
            Step = ++stepCount,
            City = startNode.Name,
            G = 0,
            H = 0,
            F = 0,
            FromCity = "-",
            Reason = "Punto de partida (inicia ola)"
        });

        while (openSet.Count > 0)
        {
            var currentName = openSet.Dequeue();

            if (currentName == targetNode.Name)
            {
                var (path, pathString) = ReconstructPath(cameFrom, currentName);
                return PathResult.Success(
                    pathString, 
                    gScore[targetNode.Name],
                    explorationSteps
                );
            }

            var neighbors = graph.GetNeighbors(currentName);
            foreach (var edge in neighbors)
            {
                var neighborName = edge.Destination.Name;
                double tentativeGScore = gScore[currentName] + edge.DistanceKm;

                if (tentativeGScore < gScore[neighborName])
                {
                    cameFrom[neighborName] = currentName;
                    gScore[neighborName] = tentativeGScore;
                    
                    openSet.Enqueue(neighborName, tentativeGScore);

                    explorationSteps.Add(new ExplorationStep
                    {
                        Step = ++stepCount,
                        City = neighborName,
                        G = gScore[neighborName],
                        H = 0,
                        F = gScore[neighborName],
                        FromCity = currentName,
                        Reason = $"g={tentativeGScore:F1} km acumulados"
                    });
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
