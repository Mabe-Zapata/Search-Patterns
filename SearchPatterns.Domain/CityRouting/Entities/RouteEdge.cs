namespace SearchPatterns.Domain.CityRouting.Entities;

/// <summary>
/// Representa una conexión o carretera entre dos ciudades (Una arista en el grafo).
/// </summary>
public class RouteEdge
{
    public CityNode Source { get; }
    public CityNode Destination { get; }
    public double DistanceKm { get; }

    public RouteEdge(CityNode source, CityNode destination, double distanceKm)
    {
        Source = source;
        Destination = destination;
        DistanceKm = distanceKm;
    }
}
