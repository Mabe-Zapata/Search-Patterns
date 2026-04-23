using SearchPatterns.Application.CityRouting.Enums;
using SearchPatterns.Application.CityRouting.Strategies;
using SearchPatterns.Domain.CityRouting.Entities;
using SearchPatterns.Domain.CityRouting.Interfaces;

namespace SearchPatterns.Application.CityRouting.Services;

/// <summary>
/// Servicio orquestador que maneja el Grafo de ciudades (El Mapa)
/// y delega el cálculo de rutas a la estrategia correspondiente elegida dinámicamente.
/// </summary>
public class CityRoutingService : ICityRoutingService
{
    private readonly CityGraph _graph;
    
    // Inyección de un diccionario o resolver para cambiar de estrategia rápidamente
    private readonly IEnumerable<IShortestPathStrategy> _strategies;

    public CityRoutingService(IEnumerable<IShortestPathStrategy> strategies)
    {
        _strategies = strategies;
        _graph = InitializeMap();
    }

    public PathResult CalculateRoute(string startCityName, string targetCityName, RoutingAlgorithmType algorithmType)
    {
        var startNode = _graph.GetNode(startCityName);
        var targetNode = _graph.GetNode(targetCityName);

        if (startNode == null) return PathResult.Failure($"La ciudad de origen '{startCityName}' no existe en el mapa.");
        if (targetNode == null) return PathResult.Failure($"La ciudad de destino '{targetCityName}' no existe en el mapa.");

        // Selección de estrategia (Aplicación Múltiple del Polimorfismo / Strategy)
        IShortestPathStrategy strategy = algorithmType switch
        {
            RoutingAlgorithmType.Dijkstra => _strategies.OfType<DijkstraStrategy>().FirstOrDefault() ?? new DijkstraStrategy(),
            RoutingAlgorithmType.AStar => _strategies.OfType<AStarStrategy>().FirstOrDefault() ?? new AStarStrategy(),
            _ => throw new ArgumentException("Algoritmo no soportado.")
        };

        return strategy.FindShortestPath(_graph, startNode, targetNode);
    }

    public IEnumerable<string> GetAvailableCities()
    {
        return _graph.Nodes.Select(n => n.Name).OrderBy(name => name);
    }

    /// <summary>
    /// Configura el mapa base con las distancias de ejemplo en Ecuador.
    /// Usamos latitudes y longitudes aproximadas para darle sentido a A*.
    /// </summary>
    private CityGraph InitializeMap()
    {
        var graph = new CityGraph();

        // 1 grado ~ 111 km, ajustamos coordenadas para que H(n) sea realista.
        var quito = new CityNode("Quito", -0.2298, -78.5249);
        var ambato = new CityNode("Ambato", -1.2416, -78.6279);
        var riobamba = new CityNode("Riobamba", -1.6669, -78.6534);
        var cuenca = new CityNode("Cuenca", -2.9001, -79.0059);
        
        // Extra cities to make map slightly more complex for A* to shine
        var banos = new CityNode("Baños", -1.3963, -78.4247);
        var guayaquil = new CityNode("Guayaquil", -2.1894, -79.8890);

        graph.AddNode(quito);
        graph.AddNode(ambato);
        graph.AddNode(riobamba);
        graph.AddNode(cuenca);
        graph.AddNode(banos);
        graph.AddNode(guayaquil);

        // Rutas establecidas
        graph.AddEdge(quito, ambato, 120);
        graph.AddEdge(ambato, riobamba, 60);
        graph.AddEdge(riobamba, cuenca, 200);
        
        // Rutas extra
        graph.AddEdge(ambato, banos, 40);
        graph.AddEdge(riobamba, banos, 80);
        graph.AddEdge(riobamba, guayaquil, 230);
        graph.AddEdge(cuenca, guayaquil, 190);

        return graph;
    }
}
