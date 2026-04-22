namespace SearchPatterns.Domain.CityRouting.Entities;

/// <summary>
/// Representa el mapa entero interconectado (El Grafo).
/// </summary>
public class CityGraph
{
    private readonly Dictionary<string, CityNode> _nodes;
    private readonly Dictionary<string, List<RouteEdge>> _adjacencyList;

    public CityGraph()
    {
        _nodes = new Dictionary<string, CityNode>();
        _adjacencyList = new Dictionary<string, List<RouteEdge>>();
    }

    public void AddNode(CityNode node)
    {
        if (!_nodes.ContainsKey(node.Name))
        {
            _nodes[node.Name] = node;
            _adjacencyList[node.Name] = new List<RouteEdge>();
        }
    }

    /// <summary>
    /// Añade una calle/ruta bidireccional entre dos ciudades
    /// </summary>
    public void AddEdge(CityNode source, CityNode destination, double distance)
    {
        AddNode(source);
        AddNode(destination);

        _adjacencyList[source.Name].Add(new RouteEdge(source, destination, distance));
        _adjacencyList[destination.Name].Add(new RouteEdge(destination, source, distance));
    }

    public CityNode? GetNode(string name)
    {
        _nodes.TryGetValue(name, out var node);
        return node;
    }

    public IEnumerable<CityNode> Nodes => _nodes.Values;

    public IEnumerable<RouteEdge> GetNeighbors(string cityName)
    {
        if (_adjacencyList.TryGetValue(cityName, out var edges))
        {
            return edges;
        }
        return Enumerable.Empty<RouteEdge>();
    }
}
