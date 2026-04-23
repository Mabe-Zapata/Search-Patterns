using SearchPatterns.Domain.CityRouting.Entities;
using SearchPatterns.Domain.CityRouting.Interfaces;

namespace SearchPatterns.Application.CityRouting.Strategies;

/// <summary>
/// Implementación concreta del Patrón Estrategia usando el algoritmo A*.
/// Explora de forma más inteligente sumando el costo acumulado (G) más una heurística (H).
/// Esto evita explorar cuadrantes innecesarios si sabe que están en la dirección equivocada.
/// </summary>
public class AStarStrategy : IShortestPathStrategy
{
    public PathResult FindShortestPath(CityGraph graph, CityNode startNode, CityNode targetNode)
    {
        var gScore = new Dictionary<string, double>();
        var fScore = new Dictionary<string, double>(); 
        var cameFrom = new Dictionary<string, string>();
        var explorationSteps = new List<ExplorationStep>();
        
        var openSet = new PriorityQueue<string, double>();

        foreach (var node in graph.Nodes)
        {
            gScore[node.Name] = double.PositiveInfinity;
            fScore[node.Name] = double.PositiveInfinity;
        }

        gScore[startNode.Name] = 0;
        fScore[startNode.Name] = HeuristicCostEstimate(startNode, targetNode);
        
        openSet.Enqueue(startNode.Name, fScore[startNode.Name]);

        int stepCount = 0;

        explorationSteps.Add(new ExplorationStep
        {
            Step = ++stepCount,
            City = startNode.Name,
            G = 0,
            H = HeuristicCostEstimate(startNode, targetNode),
            F = fScore[startNode.Name],
            FromCity = "-",
            Reason = "Punto de partida"
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
                    
                    fScore[neighborName] = gScore[neighborName] + HeuristicCostEstimate(edge.Destination, targetNode);
                    
                    openSet.Enqueue(neighborName, fScore[neighborName]);

                    explorationSteps.Add(new ExplorationStep
                    {
                        Step = ++stepCount,
                        City = neighborName,
                        G = gScore[neighborName],
                        H = HeuristicCostEstimate(edge.Destination, targetNode),
                        F = fScore[neighborName],
                        FromCity = currentName,
                        Reason = $"f={fScore[neighborName]:F1} (g={tentativeGScore:F1} + h={HeuristicCostEstimate(edge.Destination, targetNode):F1})"
                    });
                }
            }
        }

        return PathResult.Failure("No se encontró una ruta hacia el destino con A*.");
    }

    private double HeuristicCostEstimate(CityNode current, CityNode target)
    {
        return current.DistanceTo(target);
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
